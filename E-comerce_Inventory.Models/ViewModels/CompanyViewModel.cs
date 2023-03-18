using E_comerce_Inventory.Models.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.ViewModels
{
    public class CompanyViewModel
    {
        public Company Company { get; set; }

        public IEnumerable<SelectListItem> StoreList { get; set; }
    }
}
