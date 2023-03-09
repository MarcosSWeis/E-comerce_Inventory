using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_comerce_Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class BrandController :Controller
    {
        private readonly IWorkUnit _workUnit;
        public BrandController(IWorkUnit workUnit)
        {
            _workUnit = workUnit;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            Brand brand = new();
            if (id == null)
            {
                return View(brand);
            }

            brand = _workUnit.Brand.GetById(id.GetValueOrDefault());

            return View(brand);

        }


        #region API 
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _workUnit.Brand.GetAll() });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                //crando marca
                if (brand.Id == 0)
                {
                    _workUnit.Brand.Add(brand);
                } else
                {
                    _workUnit.Brand.Update(brand);
                }

                _workUnit.SaveChangesInDb();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);

        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var brandDb = _workUnit.Brand.GetById(id);
            if (brandDb == null)
            {
                return Json(new { success = false,message = "Error al borrar la marca" });
            }
            _workUnit.Brand.Delete(brandDb);
            _workUnit.SaveChangesInDb();

            return Json(new { success = true,message = "Marca borrada exitosamente" });
        }
        #endregion
    }
}
