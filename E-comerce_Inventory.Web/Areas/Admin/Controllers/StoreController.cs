using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System.Data;

namespace E_comerce_Inventory.Web.Areas.Admin.Controllers
{
    //Le indico al controlador el area al que pertenece
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]

    public class StoreController :Controller
    {
        private readonly IWorkUnit _workUnit;

        public StoreController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Store store = new Store();
            if (id == null)
            {
                //Entonces es un insert , si es asi le envio a la vista el modelo store vacio
                return View(store);
            }
            //si No entro al if entocne se esta queriendo actualizar, entoces a mi store vacia le asigno la store que estan queriendo modificar
            store = _workUnit.Store.GetById(id.GetValueOrDefault());
            if (store == null)
            {
                //si no encontro ninguna store mando un notFound
                return NotFound();
            }

            //si encontro la store la envio a la vista
            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //recibo una store del submit del formualri oque viaja por post a este metodo, de la vista Upsert.cshtml
        public IActionResult Upsert(Store store)
        {
            //pregunto si el modelo es valido , es decir si no hubo errores en nuestra pagina
            if (ModelState.IsValid)
            {
                //creando store
                if (store.Id == 0)
                {
                    _workUnit.Store.Add(store);
                } else
                {
                    //actualizar store
                    _workUnit.Store.Update(store);
                }
                _workUnit.SaveChangesInDb();
                //reutn View index
                return RedirectToAction(nameof(Index));
            }
            //return UPSERT view with the model like this
            return View(store);

        }

        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            //get all stores
            var all = _workUnit.Store.GetAll();
            //return all stores in Json format
            return Json(new { data = all });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var store = _workUnit.Store.GetById(id);
            if (store == null)
            {
                //no se encontro la store
                return Json(new { success = false,message = "Error al borrar tienda" });
            }
            _workUnit.Store.Delete(store);
            _workUnit.SaveChangesInDb();

            return Json(new { success = true,message = "Tienda borrada exitosamente" });
        }

        #endregion
    }
}
