using Microsoft.AspNetCore.Mvc;

namespace E_comerce_Inventory.Web.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class ShoppingCartController :Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
