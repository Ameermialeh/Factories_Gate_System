using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.SupplierDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class SupplierRepo : ISupplierRepo
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

        public async Task<SupplierDTO> AddSupplierAsync(CreateSupplierDTO supplierDto, int factoryId)
        {
            var supplier = new Supplier()
            {
                Name= supplierDto.Name,
                Address= supplierDto.Address,
                Phone = supplierDto.Phone,
                FactoryId = factoryId
            };

            await _appDbContext.suppliers.AddAsync(supplier);
            await _appDbContext.SaveChangesAsync();

            return new SupplierDTO
            {
                Id = supplier.SupplierId,
                Name = supplier.Name,
                Address = supplier.Address,
                Phone = supplier.Phone,
            };
        }

        public async Task<SupplierDTO?> UpdateSupplierAsync(int id, UpdateSupplierDTO dto)
        {
            var supplier = await _appDbContext.suppliers.FindAsync(id);
            if (supplier == null) return null;

            if(dto.Name != null)
            {
                supplier.Name = dto.Name;
            }
            if(dto.Address != null)
            {
                supplier.Address = dto.Address;
            }
            if(dto.Phone != null)
            {
                supplier.Phone = dto.Phone;
            }

            await _appDbContext.SaveChangesAsync();

            var supplierDto = new SupplierDTO()
            {
                Id = id,
                Name = supplier.Name,
                Address = supplier.Address,
                Phone = supplier.Phone
            };
            return supplierDto;
        }

        public async Task<bool> ChickIfSupplierExistAsync(int supplierId, int factoryId)
        {
            return await _appDbContext.suppliers.AnyAsync(s => s.SupplierId == supplierId && s.FactoryId == factoryId);
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
