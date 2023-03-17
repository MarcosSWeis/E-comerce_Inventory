using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using E_comerce_Inventory.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Web.Areas.Inventory.Controllers
{
    //Indico que Home controller partenece al area inventario
    [Area("Inventory")]
    public class HomeController :Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkUnit _workUnit;

        public HomeController(ILogger<HomeController> logger,IWorkUnit workUnit)
        {
            _logger = logger;
            _workUnit = workUnit;
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

        [ResponseCache(Duration = 0,Location = ResponseCacheLocation.None,NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
