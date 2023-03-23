using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace E_comerce_Inventory.Web.Areas.Admin.Controllers
{
    //Le indico al controlador el area al que pertenece
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]

    public class CategoryController :Controller
    {
        private readonly IWorkUnit _workUnit;

        public CategoryController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        /*
          * Como uso el mismo metodo que devuelde una categoria tanto para crear como para actualizar
          * 1°) si es nulo estoy queriedo crear una categoria => btn create category  asp-action="Upsert"> sin id
          * 2°) si no es nulo estoy queriendo actualizar => btn href="/Admin/Category/Upsert/${data}" data es el id en el js de la tabla
         */
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Category categoryDb = new Category();
            //como se va a crear una categoria le mando una nueva categoria vacia para ser completada
            if (id == null)
            {
                return View(categoryDb);
            }

            //como se va a actualizar le mando la categoria para que la actualice, es decir para rellenar los datos en los inputs
            categoryDb = _workUnit.Category.GetById(id.GetValueOrDefault());

            if (categoryDb == null)
            {
                return NotFound();
            }

            return View(categoryDb);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                //si el Id es cero estoy CREANDO, ay que el valor por defecto de un int es 0
                if (category.Id == 0)
                {
                    _workUnit.Category.Add(category);
                } else
                {
                    _workUnit.Category.Update(category);
                }
                _workUnit.SaveChangesInDb();
                return RedirectToAction(nameof(Index));
            }
            //si el modleo no es valido te devulvo la categoria que em diste
            return View(category);


        }


        #region API

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _workUnit.Category.GetAll() });

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryDb = _workUnit.Category.GetById(id);

            if (categoryDb == null)
            {
                return Json(new { success = false,message = "Error al borrar categoria" });
            }

            _workUnit.Category.Delete(categoryDb);

            _workUnit.SaveChangesInDb();

            return Json(new { success = true,message = "Categoria borrada exitosamente" });
        }
        #endregion


    }
}
