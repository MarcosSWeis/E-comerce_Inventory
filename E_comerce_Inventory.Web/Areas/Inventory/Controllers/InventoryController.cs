using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Models.ViewModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_comerce_Inventory.Web.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventory)]
    public class InventoryController :Controller
    {
        private readonly IWorkUnit _workUnit;
        private readonly ApplicationDbContext _db;
        [BindProperty] //hace que durante los cambios a la vista, los datos no se pierdan
        public InventoryVeiwModel InventoryVeiwModel { get; set; }

        public InventoryController(IWorkUnit workUnit,ApplicationDbContext db)
        {
            _workUnit = workUnit;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewInventory(int? inventoryId)
        {

            InventoryVeiwModel inventoryVeiwModel = new();

            inventoryVeiwModel.StoreList = _workUnit.Store.GetAll().ToList().Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });

            inventoryVeiwModel.ProductList = _workUnit.Product.GetAll().ToList().Select(p => new SelectListItem()
            {
                Text = p.Title,
                Value = p.Id.ToString()
            });

            inventoryVeiwModel.DetailsInventories = new List<DetailInventory>();
            if (inventoryId != null)
            {

                inventoryVeiwModel.Inventory = _workUnit.Inventory.GetById(inventoryId.GetValueOrDefault());

                inventoryVeiwModel.DetailsInventories = _workUnit.DetailInventory.GetAll(d => d.Inventory.Id == inventoryId,addProperties: $"{nameof(Product)},{nameof(Product)}.{nameof(Brand)}");
            }
            return View(inventoryVeiwModel);
        }

        public IActionResult AddQuantity(int id)
        {
            InventoryVeiwModel inventoryVeiwModel = new();

            var detail = _workUnit.DetailInventory.GetById(id);

            inventoryVeiwModel.Inventory = _workUnit.Inventory.GetFirst(d => d.Id == detail.InventoryId);

            detail.Quantity += 1;
            _workUnit.SaveChangesInDb();

            return RedirectToAction("NewInventory",new { inventoryId = inventoryVeiwModel.Inventory.Id });

        }
        [HttpPost]
        public IActionResult AddProductPost(int product,int quantity,int inventoryId) //los nombre deben ser los mismos que el name de los campos del formulario asi matchean y son recibidos de lo contrario no seran reconocidos
        {
            InventoryVeiwModel.Inventory.Id = inventoryId;
            if (InventoryVeiwModel.Inventory.Id == 0)//guarda el registro en el inventario
            {
                InventoryVeiwModel.Inventory.State = false;
                InventoryVeiwModel.Inventory.InitialDate = DateTime.Now;
                //capturo el user de la sesicon
                ClaimsIdentity claimsIdentity = (ClaimsIdentity) User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                InventoryVeiwModel.Inventory.UserAplicationId = claims.Value; //Value me da el Id del mi usuario 

                _workUnit.Inventory.Add(InventoryVeiwModel.Inventory);

                _workUnit.SaveChangesInDb();
            } else //el inventario ya existe
            {
                InventoryVeiwModel.Inventory = _workUnit.Inventory.GetById(inventoryId);
            }

            var storeProduct = _workUnit.StorePorduct.GetAll(sp => sp.ProdutId == product && sp.StoreId == InventoryVeiwModel.Inventory.StoreId,addProperties: $"{nameof(Product)}").FirstOrDefault();

            var detail = _workUnit.DetailInventory.GetAll(di => di.ProducId == product && di.InventoryId == InventoryVeiwModel.Inventory.Id,addProperties: $"{nameof(Product)}").FirstOrDefault();

            if (detail == null) //no hay dettale de ese producto en ese inventario
            {
                InventoryVeiwModel.DetailInventory = new DetailInventory();
                InventoryVeiwModel.DetailInventory.ProducId = product;
                InventoryVeiwModel.DetailInventory.InventoryId = InventoryVeiwModel.Inventory.Id;

                if (storeProduct != null)//sim hay un registro en la tienda producto
                {
                    InventoryVeiwModel.DetailInventory.OldStock = storeProduct.Quantity;
                } else //si no hay un registro en storeProduct
                {
                    InventoryVeiwModel.DetailInventory.OldStock = 0;
                }

                InventoryVeiwModel.DetailInventory.Quantity = quantity;

                _workUnit.DetailInventory.Add(InventoryVeiwModel.DetailInventory);

                _workUnit.SaveChangesInDb();


            } else //si ya hau un detalle con ese codigo
            {
                detail.Quantity += quantity;

                _workUnit.SaveChangesInDb();

            }
            //vista , prametro del mismo nombre que tiene esa vista es decir inventoryId
            return RedirectToAction("NewInventory",new { inventoryId = InventoryVeiwModel.Inventory.Id });
        }

        public IActionResult ReduceQuantity(int id)
        {
            InventoryVeiwModel inventoryVeiwModel = new();

            var detail = _workUnit.DetailInventory.GetById(id);

            inventoryVeiwModel.Inventory = _workUnit.Inventory.GetFirst(d => d.Id == detail.InventoryId);

            if (detail.Quantity == 1)
            {
                _workUnit.DetailInventory.Delete(id);
                _workUnit.SaveChangesInDb();

            } else
            {
                detail.Quantity -= 1;
                _workUnit.SaveChangesInDb();
            }

            return RedirectToAction("NewInventory",new { inventoryId = inventoryVeiwModel.Inventory.Id });


        }

        public IActionResult GenerateStock(int id)
        {
            var inventory = _workUnit.Inventory.GetFirst(i => i.Id == id);
            var detailsList = _workUnit.DetailInventory.GetAll(di => di.InventoryId == id);

            foreach (var item in detailsList)
            {

                var storeProduct = _workUnit.StorePorduct.GetAll(sp => sp.ProdutId == item.ProducId &&
                                                                       sp.StoreId == inventory.StoreId,
                                                                       addProperties: $"{nameof(Product)}").FirstOrDefault();
                if (storeProduct != null)
                {
                    storeProduct.Quantity += item.Quantity;
                } else
                {
                    storeProduct = new StoreProduct();
                    storeProduct.StoreId = inventory.StoreId;
                    storeProduct.ProdutId = item.ProducId;
                    storeProduct.Quantity = item.Quantity;
                    _workUnit.StorePorduct.Add(storeProduct);

                }
            }

            inventory.State = true;
            inventory.FinalDate = DateTime.Now;
            _workUnit.SaveChangesInDb();

            return RedirectToAction("Index");

        }

        public IActionResult Historic()
        {
            return View();
        }

        public IActionResult DetailHistoric(int id)
        {
            var detailHistoric = _workUnit.DetailInventory.GetAll(di => di.InventoryId == id,addProperties: $"{nameof(Product)},{nameof(Product)}.{nameof(Brand)}");
            return View(detailHistoric);
        }


        #region API
        [HttpGet]
        public IActionResult GetAll()
        {

            var query = _workUnit.StorePorduct.GetAll(addProperties: $"{nameof(Store)},{nameof(Product)}");
            // var query = _db.StoreProducts.Include(b => b.Store).Include(p => p.Product);
            return Json(new { data = query.ToList() });
        }



        [HttpGet]
        public IActionResult GetHistoric()
        {
            var allFields = _workUnit.Inventory.GetAll(i => i.State == true,addProperties: $"{nameof(Store)},{nameof(UserAplication)}");
            return Json(new { data = allFields });

        }
        #endregion
    }
}
