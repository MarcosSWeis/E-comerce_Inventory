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
    public class CompanyRepository :Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {

            var companyDb = _db.Companies.FirstOrDefault(c => c.Id == company.Id);

            if (companyDb != null)
            {
                if (company.LogoUrl != null)
                {
                    companyDb.LogoUrl = company.LogoUrl;
                }
                companyDb.Name = company.Name;
                companyDb.Description = company.Description;
                companyDb.City = company.City;
                companyDb.Country = company.Country;
                companyDb.Phone = company.Phone;
                companyDb.Address = company.Address;
                companyDb.StoreSaleId = company.StoreSaleId;
            }
        }
    }
}
