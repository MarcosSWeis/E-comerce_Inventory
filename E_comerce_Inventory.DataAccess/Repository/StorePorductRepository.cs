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
    public class StorePorductRepository :Repository<StoreProduct>, IStorePorductRepository
    {
        private readonly ApplicationDbContext _db;
        public StorePorductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }

}
