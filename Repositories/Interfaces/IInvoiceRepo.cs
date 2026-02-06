using FactoriesGateSystem.Models.DTOs.InvoiceDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IInvoiceRepo
    {
        Task<List<InvoiceDTO>> GetAllInvoicesAsync(Expression<Func<Invoice, bool>>? filter = null);
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<InvoiceDTO?> UpdateInvoiceAsync(int id, UpdateInvoiceDTO dto);
        Task<bool> DeleteIvnoiceAsync(int id);
    }
}
