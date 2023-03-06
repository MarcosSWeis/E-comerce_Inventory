using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository
{
    public class ProductRepoitory :Repository<Product>, IProdutRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepoitory(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var productDb = _db.Products.FirstOrDefault((p) => p.Id == product.Id);

            if (productDb != null)
            {
                if (product.ImageUrl != null)
                    productDb.ImageUrl = product.ImageUrl;


                if (product.ParentId == 0)
                    productDb.ParentId = null;
                else
                    productDb.ParentId = product.ParentId;

                productDb.SerialNumber = product.SerialNumber;
                productDb.Description = product.Description;
                productDb.Price = product.Price;
                productDb.Cost = product.Cost;
                productDb.CategoryId = product.CategoryId;
                productDb.BrandId = product.BrandId;

            }
        }
    }
}
