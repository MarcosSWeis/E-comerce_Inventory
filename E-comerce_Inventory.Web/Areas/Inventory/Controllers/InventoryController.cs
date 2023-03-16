using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace E_comerce_Inventory.Web.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventory)]
    public class InventoryController :Controller
    {
        private readonly IWorkUnit _workUnit;
        private readonly ApplicationDbContext _db;
        public InventoryController(IWorkUnit workUnit,ApplicationDbContext db)
        {
            _workUnit = workUnit;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API

        public IActionResult GetAll()
        {

            var query = _workUnit.StorePorduct.GetAll(addProperties: $"{nameof(Store)},{nameof(Product)}");
            // var query = _db.StoreProducts.Include(b => b.Store).Include(p => p.Product);
            return Json(new { data = query.ToList() });
        }

        #endregion
    }
}
