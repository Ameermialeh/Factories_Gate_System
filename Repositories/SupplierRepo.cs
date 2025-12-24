using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class SupplierRepo
    {
        private readonly AppDbContext _appDbContext;

        public SupplierRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<SupplierDTO>> GetSupplierAsync(Expression<Func<Supplier,bool>>? filter = null)
        {
            IQueryable<Supplier> supplier = _appDbContext.suppliers;
            if (filter != null)
                supplier = supplier.Where(filter);

            return await supplier.Select(s => new SupplierDTO()
            {
                SupplierId = s.SupplierId,
                SupplierName = s.SupplierName,
                Address = s.Address,
                CurrentBalance = s.CurrentBalance,
                SupplierPhone = s.SupplierPhone,
            }).ToListAsync();
        }
    }
}
