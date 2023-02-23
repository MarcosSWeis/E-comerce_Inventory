using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository
{
    public class StoreRepository :Repository<Store>, IStoreRepository
    {

        private readonly ApplicationDbContext _db;

        public StoreRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //no utilizamos el update en la interfas ya que es un metodo individual que depende de las propiedades de cada tabla 
        public void Update(Store store)
        {
            //get store actual
            var storeDb = _db.Stores.FirstOrDefault((s) => s.Id == store.Id);

            if (store != null)
            {
                storeDb.Description = store.Description;
                storeDb.Name = store.Name;
                storeDb.State = store.State;

                _db.SaveChanges();
            }
        }



    }
}
