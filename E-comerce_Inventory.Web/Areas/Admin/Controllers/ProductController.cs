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

    public class ProductController :Controller
    {
        private readonly IWorkUnit _workUnit;

        private readonly IWebHostEnvironment _hostEnviroment; //para cargar imagenes al directorio

        public ProductController(IWorkUnit workUnit,IWebHostEnvironment hostEnviroment)
        {
            _workUnit = workUnit;
            _hostEnviroment = hostEnviroment;
        }
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }


        [HttpGet]
        /*El producto esta relacionado a brand y category, para podes trabajar con
          todos esos datos asociados de ese prouto creamos un ViewModel que seria un DTO
         Un ViewModel no solo trabajara con los datos estaticos, sino que tambien trabajara con las vistas y datos de marca y categoria y no se agregara a nuesra base de datos 
         */
        public IActionResult Upsert(int? id)
        {
            //inicializamos ProductoVM
            ProductVM productVM = new()
            {
                Product = new Product(),
                ListCategory = _workUnit.Category.GetAll().Select((category) => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                }),//esto es para qeu cuadno lo edite/cree pueda seccionar a que categorias pertence el produrto, y no mandar todos los modelos a la vista
                ListBrand = _workUnit.Brand.GetAll().Select((brand) => new SelectListItem
                {
                    Text = brand.Name,
                    Value = brand.Id.ToString(),
                }),
                ListParent = _workUnit.Product.GetAll().Select((parent) => new SelectListItem
                {
                    Text = parent.Title,
                    Value = parent.Id.ToString(),
                })

            };

            if (id == null)
                return View(productVM);//enviamos para crear el nuevo producto

            productVM.Product = _workUnit.Product.GetById(id.GetValueOrDefault());

            if (productVM.Product == null)
                return NotFound();


            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(ProductVM productVM)
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
                    var uploads = Path.Combine(webRoootPath,@"img\products");//combino  los dos tring en la ruta donde se alojaran las imagens en este servidor
                    var extensionFile = Path.GetExtension(files[0].FileName);//Obtengo la extenxion de mi archivo
                    if (productVM.Product.ImageUrl != null)
                    {
                        //esto es para editar, por ende necesitamso borra la imagen anterior
                        var imgPath = Path.Combine(webRoootPath,productVM.Product.ImageUrl.TrimStart('\\'));
                        DeleteImage(imgPath);
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads,fileName + extensionFile),FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }
                    productVM.Product.ImageUrl = @"\img\products\" + fileName + extensionFile;

                } else
                {
                    //si actualizo el resto de campos menos la imagen
                    if (productVM.Product.Id != 0)//se esta actualizando
                    {
                        Product productDB = _workUnit.Product.GetById(productVM.Product.Id);

                        productVM.Product.ImageUrl = productDB.ImageUrl;

                    }
                }


                if (productVM.Product.Id == 0)
                {
                    var a = productVM.ListBrand;
                    var brand = HttpContext.Request.Form;
                    _workUnit.Product.Add(productVM.Product);

                } else
                {
                    _workUnit.Product.Update(productVM.Product);
                }


                _workUnit.SaveChangesInDb();
                return RedirectToAction(nameof(Index));

            } else //Model invalid
            {
                productVM.ListCategory = _workUnit.Category.GetAll().Select((category) => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString(),
                });
                productVM.ListBrand = _workUnit.Brand.GetAll().Select((brand) => new SelectListItem
                {
                    Text = brand.Name,
                    Value = brand.Id.ToString(),
                });
                productVM.ListParent = _workUnit.Product.GetAll().Select((parent) => new SelectListItem
                {
                    Text = parent.Title,
                    Value = parent.Id.ToString(),
                });

                if (productVM.Product.Id != 0)//actualizar
                {
                    productVM.Product = _workUnit.Product.GetById(productVM.Product.Id);
                }
            }

            return View(productVM);

        }


        #region API

        [HttpGet]
        public IActionResult GetAll()
        {

            return Json(new { data = _workUnit.Product.GetAll(addProperties: "Category,Brand") });

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productDb = _workUnit.Product.GetById(id);

            if (productDb == null)
                return Json(new { success = false,message = "Error al borrar producto" });

            string webRoootPath = _hostEnviroment.WebRootPath;

            var imgPath = Path.Combine(webRoootPath,productDb.ImageUrl.TrimStart('\\'));
            DeleteImage(imgPath);

            _workUnit.Product.Delete(productDb);


            _workUnit.SaveChangesInDb();

            return Json(new { success = true,message = "Producto borrado exitosamente" });
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