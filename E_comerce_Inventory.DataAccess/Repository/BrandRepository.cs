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
    public class BrandRepository :Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;
        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Brand brand)
        {
            var brandDb = _db.Brands.FirstOrDefault((b) => b.Id == brand.Id);
            if (brandDb != null)
            {
                brandDb.Name = brand.Name;
                brandDb.State = brand.State;
            }
        }
    }
}
