using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository
{
    public class WorkUnit :IWorkUnit
    {

        private readonly ApplicationDbContext _db;
        public IStoreRepository Store { get; private set; }
        public ICategoryRepository Category { get; private set; }

        public IBrandRepository Brand { get; private set; }

        public IProdutRepository Product { get; private set; }
        public IUserAplicationRepository UserAplication { get; private set; }
        public WorkUnit(ApplicationDbContext db)
        {
            _db = db;
            Store = new StoreRepository(_db); //we initialize
            Category = new CategoryRepository(_db);//we initialize
            Brand = new BrandRepository(_db);//we initialize
            Product = new ProductRepoitory(_db);//we initialize
            UserAplication = new UserAplicationRepository(_db);
        }


        /// <summary>
        /// close processes after use
        /// </summary>
        public void Dispose()
        {
            _db.Dispose();
        }

        /// <summary>
        /// Save changes realicing on the data base
        /// </summary>
        public void SaveChangesInDb()
        {
            _db.SaveChanges();
        }


    }
}
