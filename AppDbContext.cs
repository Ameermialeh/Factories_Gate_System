using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoriesGateSystem
{
    public class AppDbContext:DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options):base(options) { 
            
        }
        DbSet<User> users { get; set; }

    }
}
