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
                Id = s.SupplierId,
                Name = s.SupplierName,
                Address = s.Address,
                CurrentBalance = s.CurrentBalance,
                Phone = s.SupplierPhone,
            }).ToListAsync();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _appDbContext.suppliers.FirstOrDefaultAsync(s => s.SupplierId == id);
        }

        public async Task<SupplierDTO> AddSupplierAsync(SupplierDTO supplierDto)
        {
            var supplier = new Supplier()
            {
                SupplierName= supplierDto.Name,
                Address=supplierDto.Address,
                SupplierPhone = supplierDto.Phone,
                CurrentBalance = supplierDto.CurrentBalance,
            };

            await _appDbContext.suppliers.AddAsync(supplier);
            await _appDbContext.SaveChangesAsync();
            supplierDto.Id = supplier.SupplierId;
            return supplierDto;
        }
    }
}
