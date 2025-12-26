using FactoriesGateSystem.DTOs.SupplierDTOs;
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
                Name = s.Name,
                Address = s.Address,
                Phone = s.Phone,
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
                Name= supplierDto.Name,
                Address= supplierDto.Address!,
                Phone = supplierDto.Phone!,
            };

            await _appDbContext.suppliers.AddAsync(supplier);
            await _appDbContext.SaveChangesAsync();
            supplierDto.Id = supplier.SupplierId;
            return supplierDto;
        }

        public async Task<SupplierDTO?> UpdateSupplierAsync(int id, UpdateSupplierDTO UpdateSupplierDto)
        {
            var supplier = await _appDbContext.suppliers.FindAsync(id);
            if (supplier == null)
                return null;

            supplier.Name = UpdateSupplierDto.Name;
            supplier.Phone = UpdateSupplierDto.Phone!;
            supplier.Address = UpdateSupplierDto.Address!;

            await _appDbContext.SaveChangesAsync();

            var supplierDto = new SupplierDTO()
            {
                Id = id,
                Name = UpdateSupplierDto.Name,
                Address = UpdateSupplierDto.Address,
                Phone = UpdateSupplierDto.Phone
            };
            return supplierDto;
        }

        public async Task<Supplier?> DeleteSupplierAsync(int id)
        {
            var supplier = await _appDbContext.suppliers.FindAsync(id);
            if (supplier == null)
                return null;

            _appDbContext.Remove(supplier);
            await _appDbContext.SaveChangesAsync();
            return supplier;
        }
    }
}
