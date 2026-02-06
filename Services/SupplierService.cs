using FactoriesGateSystem.Models.DTOs.SupplierDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepo _supplierRepo;
        private readonly ICookieService _cookieService;
        public SupplierService(ISupplierRepo supplierRepo, ICookieService cookieService)
        {
            _supplierRepo = supplierRepo;
            _cookieService = cookieService;
        }

        public async Task<List<SupplierDTO>> GetAllSuppliers()
        {
            var supplier = await _supplierRepo.GetSupplierAsync();
            return supplier;
        }
        public async Task<List<SupplierDTO>> GetAllSuppliersWithNameAsync(string name)
        {
            var filtered = await _supplierRepo.GetSupplierAsync(s => s.Name.Contains(name));
            return filtered;
        }
        public async Task<SupplierDTO> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepo.GetSupplierByIdAsync(id)
                ?? throw new BadHttpRequestException("Supplier Not Found", StatusCodes.Status404NotFound);

            var supplierDto = new SupplierDTO()
            {
                Id = id,
                Name = supplier.Name,
                Address = supplier.Address,
                Phone = supplier.Phone,
            };
            return supplierDto;
        }
        public async Task<List<SupplierDTO>> GetSuppliersByNameAsync(string name)
        {
            var supplier = await _supplierRepo.GetSupplierAsync(s => s.Name.Contains(name));
            return supplier;
        }
        public async Task<SupplierDTO> AddSupplierAsync(CreateSupplierDTO dto)
        {

            var factoryId = _cookieService.Get("FactoryId")
               ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var supplier = await _supplierRepo.AddSupplierAsync(dto, int.Parse(factoryId));
            return supplier;

        }
        public async Task<SupplierDTO> UpdateSupplierAsync(int id, UpdateSupplierDTO dto)
        {
            var supplier = await _supplierRepo.UpdateSupplierAsync(id, dto)
                ?? throw new BadHttpRequestException("Supplier Not Found", StatusCodes.Status404NotFound);
            return supplier;
        }
        public async Task<DeleteSupplierDTO> DeleteSupplierAsync(int id)
        {
            var supplier = await _supplierRepo.DeleteSupplierAsync(id)
                ?? throw new BadHttpRequestException("Supplier Not Found", StatusCodes.Status404NotFound);

            var supplierDto = new DeleteSupplierDTO()
            {
                Id = id,
                Name = supplier.Name,
            };
            return supplierDto;
        }
    }
}
