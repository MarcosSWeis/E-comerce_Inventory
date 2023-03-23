using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Models.ViewModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace E_comerce_Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventory)]

    public class CompanyController :Controller
    {
        private readonly IWorkUnit _workUnit;

        private readonly IWebHostEnvironment _hostEnviroment; //para cargar imagenes al directorio

        public CompanyController(IWorkUnit workUnit,IWebHostEnvironment hostEnviroment)
        {
            _workUnit = workUnit;
            _hostEnviroment = hostEnviroment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var company = _workUnit.Company.GetAll();
            return View(company);
        }


        [HttpGet]
        /*El producto esta relacionado a brand y category, para podes trabajar con
          todos esos datos asociados de ese prouto creamos un ViewModel que seria un DTO
         Un ViewModel no solo trabajara con los datos estaticos, sino que tambien trabajara con las vistas y datos de marca y categoria y no se agregara a nuesra base de datos 
         */
        public IActionResult Upsert(int? id)
        {
            //inicializamos ProductoVM
            CompanyViewModel companyVM = new()
            {
                Company = new Company(),
                StoreList = _workUnit.Store.GetAll().Select((s) => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                }),//esto es para qeu cuadno lo edite/cree pueda seccionar a que categorias pertence el produrto, y no mandar todos los modelos a la vista


            };

            if (id == null)
                return View(companyVM);//enviamos para crear el nuevo producto

            companyVM.Company = _workUnit.Company.GetById(id.GetValueOrDefault());

            if (companyVM.Company == null)
                return NotFound();


            return View(companyVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(CompanyViewModel companyVM)
        {
            if (ModelState.IsValid)
            {
                //load image

                string webRoootPath = _hostEnviroment.WebRootPath;//obtiene la ruta del directorio raíz del servidor web.
                var files = HttpContext.Request.Form.Files;//obtiene la colección de archivos que se han enviado al servidor web.
                //contain images
                if (files.Count > 0)// verifico si se han enviado archivos al servidor web.
                {
                    string fileName = Guid.NewGuid().ToString();//genero el nombre de mi archivo con un ID unico
                    var uploads = Path.Combine(webRoootPath,@"img\companies");//combino  los dos tring en la ruta donde se alojaran las imagens en este servidor
                    var extensionFile = Path.GetExtension(files[0].FileName);//Obtengo la extenxion de mi archivo
                    if (companyVM.Company.LogoUrl != null)
                    {
                        //esto es para editar, por ende necesitamso borra la imagen anterior
                        var imgPath = Path.Combine(webRoootPath,companyVM.Company.LogoUrl.TrimStart('\\'));
                        DeleteImage(imgPath);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName + extensionFile),FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    companyVM.Company.LogoUrl = @"\img\companies\" + fileName + extensionFile;

                } else
                {
                    //si actualizo el resto de campos menos la imagen
                    if (companyVM.Company.Id != 0)//se esta actualizando
                    {
                        Company companyDB = _workUnit.Company.GetById(companyVM.Company.Id);

                        companyVM.Company.LogoUrl = companyDB.LogoUrl;

                    }
                }


                if (companyVM.Company.Id == 0)
                {
                    _workUnit.Company.Add(companyVM.Company);

                } else
                {
                    _workUnit.Company.Update(companyVM.Company);
                }


                _workUnit.SaveChangesInDb();
                return RedirectToAction(nameof(Index));

            } else //Model invalid
            {
                companyVM.StoreList = _workUnit.Category.GetAll().Select((s) => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                });

                if (companyVM.Company.Id != 0)//actualizar
                {
                    companyVM.Company = _workUnit.Company.GetById(companyVM.Company.Id);
                }
            }

            return View(companyVM.Company);

        }


        #region API

        [HttpGet]
        public IActionResult GetAll()
        {

            return Json(new { data = _workUnit.Company.GetAll(addProperties: $"{nameof(Store)}") });

        }

        #endregion

        private void DeleteImage(string imgPath)
        {
            if (System.IO.File.Exists(imgPath))//si existe la imagen en este entrorno y en ese directorio, a borro
            {
                System.IO.File.Delete(imgPath);
            }
        }

    }
}