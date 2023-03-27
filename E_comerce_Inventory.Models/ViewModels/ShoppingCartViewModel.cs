using E_comerce_Inventory.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public Company Company { get; set; }
        public StoreProduct StoreProduct { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public Order Order { get; set; }

        public IEnumerable<OrderDetail> OrderDetailList { get; set; }

    }
}
