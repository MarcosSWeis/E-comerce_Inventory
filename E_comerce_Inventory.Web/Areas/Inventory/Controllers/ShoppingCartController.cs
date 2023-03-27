using DinkToPdf;
using DinkToPdf.Contracts;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Models.ViewModels;
using E_comerce_Inventory.Utilities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly IConverter _convert;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(IWorkUnit workUnit,IEmailSender emailSender,UserManager<IdentityUser> userManager,IConverter converter)
        {
            _workUnit = workUnit;
            _emailSender = emailSender;
            _userManager = userManager;
            _convert = converter;

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


        public IActionResult ProceedWithThePayment()
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

            ShoppingCartVM.Order.Name = ShoppingCartVM.Order.UserAplication.Name;
            ShoppingCartVM.Order.LastName = ShoppingCartVM.Order.UserAplication.LastName;
            ShoppingCartVM.Order.Country = ShoppingCartVM.Order.UserAplication.Country;

            return View(ShoppingCartVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ProceedWithThePayment(Order order)
        {
            ShoppingCartViewModel CartVM = LoadViewModelWhitData(order);
            if (!ModelState.IsValid)
            {
                return View(order);
            }

            TempData.Put("datashoppingcartpayment",CartVM); //Con la propiedad [BindProperty] me ahorraba hacer lo del tempData ,
                                                            //porque cuando los valores de una accion pasean a otra por post se mantengan las propiedades   

            return RedirectToAction(nameof(Payment));
        }


        public IActionResult Payment()
        {


            ShoppingCartViewModel CartVM = TempData.Get<ShoppingCartViewModel>("datashoppingcartpayment");
            TempData.Keep("datashoppingcartpayment");
            //el model es valido aca porque se realizo al validacion en la vista anterior
            return View(CartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Payment(string stripeToken)
        {
            var userAplicationId = GetClaim().Value;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Order = new Order(),

            };
            ShoppingCartVM.Order.UserAplication = _workUnit.UserAplication.GetFirst(ua => ua.Id == userAplicationId);
            ShoppingCartVM.ShoppingCartList = _workUnit.ShoppingCart.GetAll(sc => sc.UserAplicationId == userAplicationId,addProperties: $"{nameof(Product)}");


            ShoppingCartVM.Order.OrderState = StateOrder.Pending.ToString();
            ShoppingCartVM.Order.PaymentStatus = StatePayment.Pending.ToString();
            ShoppingCartVM.Order.UserAplicationId = userAplicationId;
            ShoppingCartVM.Order.OrderDate = DateTime.Now;
            ShoppingCartVM.Company = _workUnit.Company.GetFirst();

            ShoppingCartViewModel CartVM = TempData.Get<ShoppingCartViewModel>("datashoppingcartpayment");

            ShoppingCartVM.Order.Address = CartVM.Order.Address;
            ShoppingCartVM.Order.City = CartVM.Order.City;
            ShoppingCartVM.Order.Dni = CartVM.Order.Dni;
            ShoppingCartVM.Order.Phone = CartVM.Order.Phone;
            ShoppingCartVM.Order.PostalCode = CartVM.Order.PostalCode;
            ShoppingCartVM.Order.LastName = CartVM.Order.LastName;
            ShoppingCartVM.Order.Name = CartVM.Order.Name;
            ShoppingCartVM.Order.Country = CartVM.Order.Country;


            _workUnit.Order.Add(ShoppingCartVM.Order);
            _workUnit.SaveChangesInDb();

            foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = shoppingCart.ProductId,
                    OrderId = ShoppingCartVM.Order.Id,
                    Price = shoppingCart.Product.Price,
                    Quantity = shoppingCart.Quantity,
                };
                //actualizo el total de la orden por cada producto que recorro de mi carrito
                ShoppingCartVM.Order.OrderTotal += (orderDetail.Price * orderDetail.Quantity);
                //a cada orden formada la guardo en la base de datos
                _workUnit.OrderDetail.Add(orderDetail);
            }

            //borrlo todos los productos de mi carrito ya que se realizo la orden
            _workUnit.ShoppingCart.DeleteRange(ShoppingCartVM.ShoppingCartList);
            // _workUnit.SaveChangesInDb();
            //actualizo la session 
            UpdateSesion(0);

            if (stripeToken != null)

            {
                var options = new Stripe.ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.Order.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Numero de Orden: " + ShoppingCartVM.Order.Id,
                    Source = stripeToken
                };

                var service = new Stripe.ChargeService();
                Stripe.Charge charge = service.Create(options);

                if (charge.BalanceTransactionId == null) //la tansaccion no se ralizo correctamenre
                {
                    ShoppingCartVM.Order.PaymentStatus = StatePayment.Refused.ToString();
                } else
                {
                    ShoppingCartVM.Order.TransactionId = charge.Id;
                }

                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.Order.OrderState = StateOrder.Aproved.ToString();
                    ShoppingCartVM.Order.PaymentStatus = StatePayment.Aproved.ToString();
                    ShoppingCartVM.Order.PaymentDate = DateTime.Now;

                    //descontar stock de productos
                    foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
                    {

                        var product = _workUnit.StorePorduct.GetFirst(sp => sp.ProdutId == shoppingCart.ProductId && sp.StoreId == ShoppingCartVM.Company.StoreSaleId);
                        if (product != null)
                        {
                            product.Quantity -= shoppingCart.Quantity;
                        }

                    }

                }

            }
            _workUnit.SaveChangesInDb();
            return RedirectToAction("ConfirmOrder","ShoppingCart",new { id = ShoppingCartVM.Order.Id });
        }

        public IActionResult ConfirmOrder(int id)
        {
            return View(id);
        }

        public IActionResult ViewToPfdPage(int id)
        {
            ShoppingCartVM = new ShoppingCartViewModel();
            ShoppingCartVM.Company = _workUnit.Company.GetFirst();
            var order = _workUnit.Order.GetFirst(o => o.Id == id,addProperties: $"{nameof(UserAplication)}");
            ShoppingCartVM.Order = _workUnit.Order.GetFirst(o => o.Id == id,addProperties: $"{nameof(UserAplication)}");
            ShoppingCartVM.OrderDetailList = _workUnit.OrderDetail.GetAll(od => od.OrderId == id,addProperties: $"{nameof(Product)}");

            return View(ShoppingCartVM);

        }

        public IActionResult ShowDetailOrderPdfInPage(int id)
        {
            byte[] pdfFile = CreatePdfOfTheOrder(id,"Inventory/ShoppingCart/ViewToPfdPage");
            return File(pdfFile,"application/pdf");
        }

        public IActionResult DownloadFilePdfOfTheOrder(int id)
        {
            byte[] pdfFile = CreatePdfOfTheOrder(id,"Inventory/ShoppingCart/ViewToPfdPage");
            string namePdf = "Orden#" + id + "_" + DateTime.Now.ToString("ddMMyyyy");
            return File(pdfFile,"application/pdf",namePdf);
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

        private byte[] CreatePdfOfTheOrder(int orderId,string urlToGo)
        {
            var pdfConfigure = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings()
                    {
                      Page = ChangePathUrl(urlToGo,orderId)
                    }
                }
            };
            return _convert.Convert(pdfConfigure);
        }
        /// <summary>
        /// Retorna un string de la url a la que decea ir
        /// </summary>
        /// <param name="urlToGo">Tiene una forma Controller/Method</param>
        /// <returns></returns>
        private string ChangePathUrl(string urlToGo,int orderId)
        {
            string current_url = HttpContext.Request.Path; ///Inventory/ShoppingCart/ShowDetailOrderPdfInPage/12
            string url_page = HttpContext.Request.GetEncodedUrl(); //ejemplo localhst:/exetera
            //completo la ruta
            url_page = url_page.Replace(current_url,"");
            url_page = $"{url_page}/{urlToGo}/{orderId}";

            return url_page;
        }

        private ShoppingCartViewModel LoadViewModelWhitData(Order order)
        {
            ShoppingCartViewModel shoppingCartVM = new ShoppingCartViewModel()
            {
                Order = order

            };
            var userAplicationId = GetClaim().Value;
            shoppingCartVM.Order.OrderTotal = 0;
            shoppingCartVM.Order.UserAplication = _workUnit.UserAplication.GetFirst(ua => ua.Id == userAplicationId);
            shoppingCartVM.ShoppingCartList = _workUnit.ShoppingCart.GetAll(sc => sc.UserAplicationId == userAplicationId,addProperties: $"{nameof(Product)}");

            foreach (var list in shoppingCartVM.ShoppingCartList)
            {
                shoppingCartVM.Order.OrderTotal += (list.Product.Price * list.Quantity);
            }
            shoppingCartVM.Order.Name = shoppingCartVM.Order.UserAplication.Name;
            shoppingCartVM.Order.LastName = shoppingCartVM.Order.UserAplication.LastName;
            shoppingCartVM.Order.Country = shoppingCartVM.Order.UserAplication.Country;

            return shoppingCartVM;
        }
        #endregion
    }
}

