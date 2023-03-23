using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using EllipticCurve.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository
{
    public class ShoppingCartRepository :Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart shoppingCart)
        {

            _db.Update(shoppingCart);
            //var shoppingCartDB = _db.ShoppingCarts.FirstOrDefault((o) => o.Id == shoppingCart.Id);
            //if (shoppingCartDB != null)
            //{

            //}
        }
    }
}
