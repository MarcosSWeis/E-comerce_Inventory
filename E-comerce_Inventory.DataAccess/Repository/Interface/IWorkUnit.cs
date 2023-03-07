using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.DataAccess.Repository.Interface
{
    public interface IWorkUnit :IDisposable
    {
        //variable  de tipo IStoreRepository
        IStoreRepository Store { get; }

        ICategoryRepository Category { get; }

        IBrandRepository Brand { get; }

        IProdutRepository Product { get; }

        IUserAplicationRepository UserAplication { get; }

        public void SaveChangesInDb();

    }
}
