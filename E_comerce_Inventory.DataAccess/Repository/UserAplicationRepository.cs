using E_comerce_Inventory.DataAccess.Data;
using E_comerce_Inventory.DataAccess.Repository.Interface;
using E_comerce_Inventory.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository
{
    public class UserAplicationRepository :Repository<UserAplication>, IUserAplicationRepository
    {
        private readonly ApplicationDbContext _db;
        public UserAplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(UserAplication userAplication)
        {
            _db.Update(userAplication);
        }
    }
}
