using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Models.ViewModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace E_comerce_Inventory.Web.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ShoppingCartController :Controller
    {
        private readonly IWorkUnit _workUnit;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(IWorkUnit workUnit,IEmailSender emailSender,UserManager<IdentityUser> userManager)
        {
            _workUnit = workUnit;
            _emailSender = emailSender;
            _userManager = userManager;


        }
        public IActionResult Index()
        {
            var userAplicationId = GetClaim().Value;

            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Order = new Order(),//inicializo la orden
                ShoppingCartList = _workUnit.ShoppingCart.GetAll(sc => sc.UserAplicationId == userAplicationId,addProperties: $"{nameof(Product)}") //todos los productos con sus cantidades
            };
            //sigo inicializando la orden
            ShoppingCartVM.Order.OrderTotal = 0;
            ShoppingCartVM.Order.UserAplication = _workUnit.UserAplication.GetFirst(ua => ua.Id == userAplicationId);

            foreach (var list in ShoppingCartVM.ShoppingCartList)
            {
                //price es una propieda no mapeada en la base de datos
                //De cada carrito voy accediendo a su correspondiente producto asignado y le guerdo el precio en price                
                ShoppingCartVM.Order.OrderTotal += (list.Product.Price * list.Quantity);
            }

            return View(ShoppingCartVM);
        }


        public IActionResult AddProductShoppingCart(int shoppingcartid)
        {
            var shoppingCart = _workUnit.ShoppingCart.GetFirst(sc => sc.Id == shoppingcartid);
            shoppingCart.Quantity++;
            _workUnit.SaveChangesInDb();

            return RedirectToAction(nameof(Index)); //redirijo al index para que vualva a cargar la vista con tododo actualizado

        }
        public IActionResult ReduceProductShoppingCart(int shoppingcartid)
        {
            var shoppingCart = _workUnit.ShoppingCart.GetFirst(sc => sc.Id == shoppingcartid,addProperties: $"{nameof(Product)}");
            if (shoppingCart.Quantity == 1)
            {
                DelteProductOfCart(shoppingCart);

                var numberProducts = _workUnit.ShoppingCart.GetAll(sc => sc.UserAplicationId == shoppingCart.UserAplicationId).ToList().Count(); //cantida de productos que tiene el usuario
                UpdateSesion(numberProducts);
            } else
            {
                shoppingCart.Quantity--;
                _workUnit.SaveChangesInDb();
            }

            return RedirectToAction(nameof(Index));//redirijo al index para que vualva a cargar la vista con tododo actualizado

        }
        public IActionResult DeleteProductOfShoppingCart(int shoppingcartid)
        {
            var shoppingCart = _workUnit.ShoppingCart.GetFirst(sc => sc.Id == shoppingcartid,addProperties: $"{nameof(Product)}");
            DelteProductOfCart(shoppingCart);
            var numberProducts = _workUnit.ShoppingCart.GetAll(sc => sc.UserAplicationId == shoppingCart.UserAplicationId).ToList().Count();//cantida de productos que tiene el usuario       
            UpdateSesion(numberProducts);

            return RedirectToAction(nameof(Index));//redirijo al index para que vualva a cargar la vista con tododo actualizado

        }


        #region PRIVATE
        private void DelteProductOfCart(ShoppingCart shoppingcart)
        {
            _workUnit.ShoppingCart.Delete(shoppingcart);
            _workUnit.SaveChangesInDb();
        }
        private void UpdateSesion(int newValueSession)
        {
            HttpContext.Session.SetInt32(DS.ssShoppingCart,newValueSession);
        }

        private Claim GetClaim()
        {
            var claimIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim;
        }
        #endregion
    }
}

