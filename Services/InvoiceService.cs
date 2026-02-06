using FactoriesGateSystem.Models.DTOs.InvoiceDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepo _invoiceRepo;
        
        public InvoiceService(IInvoiceRepo invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }
        public async Task<List<InvoiceDTO>> GetAllInvoicesAsync()
        {
            var invoice = await _invoiceRepo.GetAllInvoicesAsync();
            return invoice;
        }
        public async Task<List<InvoiceDTO>> GetAllInvoicesWithFilterAsync(int orderId)
        {
            var filtered = await _invoiceRepo.GetAllInvoicesAsync(i => i.OrderId == orderId);
            return filtered;
        }
        public async Task<InvoiceDTO> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _invoiceRepo.GetInvoiceByIdAsync(id)
                ?? throw new BadHttpRequestException("No Invoices found", StatusCodes.Status404NotFound);

            var invoiceDto = new InvoiceDTO()
            {
                Id = id,
                Total = invoice.Total,
                Date = invoice.Date,
                OrderId = invoice.OrderId,

            };
            return invoiceDto;
        }
        public async Task<InvoiceDTO> UpdateInvoiceAsync(int id, UpdateInvoiceDTO dto)
        {
            var invoice = await _invoiceRepo.UpdateInvoiceAsync(id, dto)
              ?? throw new BadHttpRequestException("Invoice not found", StatusCodes.Status404NotFound);

            return invoice;
        }
        public async Task DeleteIvnoiceAsync(int id)
        {
            var done = await _invoiceRepo.DeleteIvnoiceAsync(id);
            if (!done) { throw new BadHttpRequestException("Invoice not found", StatusCodes.Status404NotFound); }
        }
    }
}
