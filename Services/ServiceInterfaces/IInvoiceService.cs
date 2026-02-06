using FactoriesGateSystem.Models.DTOs.InvoiceDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IInvoiceService
    {
        Task<List<InvoiceDTO>> GetAllInvoicesAsync();
        Task<List<InvoiceDTO>> GetAllInvoicesWithFilterAsync(int orderId);
        Task<InvoiceDTO> GetInvoiceByIdAsync(int id);
        Task<InvoiceDTO> UpdateInvoiceAsync(int id, UpdateInvoiceDTO dto);
        Task DeleteIvnoiceAsync(int id);
    }
}
