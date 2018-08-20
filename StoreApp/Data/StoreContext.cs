using Microsoft.EntityFrameworkCore;
using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<StorageProducts> StorageProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>().ToTable("Suppliers");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
