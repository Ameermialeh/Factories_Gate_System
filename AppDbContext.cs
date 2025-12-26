using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FactoriesGateSystem
{
    public class AppDbContext:DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options):base(options) { 
             
        }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Invoice> invoices { get; set; }
        public DbSet<Vacation> vacations { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Material> materials { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Salary> salaries { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Manager> manager { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }  
        public DbSet<MaterialPurchase> MaterialPurchase { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Factory> factory { get; set; }
        public DbSet<Expense> expenses { get; set; }
    }
}
