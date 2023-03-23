using E_comerce_Inventory.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository.Interface
{
    public interface IProdutRepository :IRepository<Product>
    {
        public void Update(Product product);
    }
}
