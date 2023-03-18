using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Web.Areas.Inventory.Controllers
{
    //Indico que Home controller partenece al area inventario
    [Area("Inventory")]
    public class HomeController :Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkUnit _workUnit;

        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public HomeController(ILogger<HomeController> logger,IWorkUnit workUnit,ApplicationDbContext db)
        {
            _logger = logger;
            _workUnit = workUnit;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _workUnit.Product.GetAll(addProperties: $"{nameof(Category)},{nameof(Brand)}");
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] //tiene que estar registrado
        public IActionResult Detail(ShoppingCartViewModel shoppingCartVm)
        {
            //user of de session
            var claimIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVm.ShoppingCart.UserAplicationId = claim.Value;

            //miro en la db si este producto ya esta en el carro de usuario

            ShoppingCart shoppingCartDb = _workUnit.ShoppingCart.GetFirst(sc => sc.UserAplicationId == shoppingCartVm.ShoppingCart.UserAplicationId &&
                                                                                sc.ProductId == shoppingCartVm.ShoppingCart.ProductId,
                                                                                addProperties: $"{nameof(Product)}");

            if (shoppingCartDb == null)
            {
                //este usuario no tieen este producto en su carrito
                _workUnit.ShoppingCart.Add(shoppingCartVm.ShoppingCart);
            } else // si ya tiene el registro
            {
                shoppingCartDb.Quantity += shoppingCartVm.ShoppingCart.Quantity;
                _workUnit.ShoppingCart.Update(shoppingCartDb);
            }
            _workUnit.SaveChangesInDb();

            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            ShoppingCartVM = new ShoppingCartViewModel();
            ShoppingCartVM.Company = _workUnit.Company.GetFirst(); //SOLO EXISTE UNA EN MI PROYECTO
            //miro si en mi store esta ese producto
            ShoppingCartVM.StoreProduct = _workUnit.StorePorduct.GetFirst(sp => sp.Product.Id == id && sp.StoreId == ShoppingCartVM.Company.StoreSaleId,
                                                                         addProperties: $"{nameof(Product)},{nameof(Product)}.{nameof(Category)},{nameof(Product)}.{nameof(Brand)}");

            if (ShoppingCartVM.StoreProduct == null) //compruebo si esta ese producto en la tiebda
            {
                return RedirectToAction("Index");
            } else //si hay producto inicializo el carro de compras
            {
                ShoppingCartVM.ShoppingCart = new ShoppingCart()
                {
                    Product = ShoppingCartVM.StoreProduct.Product,
                    ProductId = ShoppingCartVM.StoreProduct.ProdutId,
                };

                return View(ShoppingCartVM); //voy a detalle

            }




        }

        [ResponseCache(Duration = 0,Location = ResponseCacheLocation.None,NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
