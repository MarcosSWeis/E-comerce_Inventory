using E_comerce_Inventory.Models.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.ViewModels
{
    public class ProductVM
    {
        //agrego todo lo que necesito para luego mostrar en la vista
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> ListCategory { get; set; }
        public IEnumerable<SelectListItem> ListBrand { get; set; }
    }
}
