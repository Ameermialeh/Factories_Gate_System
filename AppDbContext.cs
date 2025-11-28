using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FactoriesGateSystem
{
    public class AppDbContext:DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options):base(options) { 
             
        }
        public DbSet<Order> orders { get; set; }
        public DbSet<Vacation> vacations { get; set; }
        public DbSet<WorkPlan> workPlans { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Material> materials { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Customer> customer { get; set; }

        public DbSet<OrderProduct> orderProducts { get; set; }  
        public DbSet<EmployeeMaterial> employeeMaterials { get; set; }
        public DbSet<SupplierMaterial> supplierMaterials { get; set; }



    }
}
