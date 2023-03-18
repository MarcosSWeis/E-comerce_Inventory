using E_comerce_Inventory.Models.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace E_comerce_Inventory.DataAccess.Data
{
    public class ApplicationDbContext :IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Store> Stores { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserAplication> UsersAplication { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<DetailInventory> DetailInventories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrdersDetail { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }




    }
}
