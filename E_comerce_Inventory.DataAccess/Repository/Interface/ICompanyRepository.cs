using E_comerce_Inventory.Models.DataModels;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository.Interface
{
    public interface ICompanyRepository :IRepository<Company>
    {
        public void Update(Company company);
    }
}
