using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.InvoiceDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static FactoriesGateSystem.Models.DTOs.InvoiceDTOs.UpdateInvoiceDTO;

namespace FactoriesGateSystem.Repositories
{
    public class InvoiceRepo
    {
        private readonly AppDbContext _appDbContext;

        public InvoiceRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<InvoiceDTO>> GetAllInvoicesAsync(Expression<Func<Invoice, bool>>? filter = null)
        {
            IQueryable<Invoice> query = _appDbContext.invoices;
            if (filter != null)
            {
                query.Where(filter);
            }
            return await query.Select(i=> new InvoiceDTO
            {
                Id = i.InvoiceId,
                Date = i.Date,
                Total = i.Total,
                OrderId = i.OrderId,
            }).ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
        {
            return await _appDbContext.invoices.Where(i => i.InvoiceId == id).FirstOrDefaultAsync();
        }


        public async Task<InvoiceDTO?> UpdateInvoiceDateAsync(UpdateInvoiceDateDTO dto)
        {
            var invoice = await GetInvoiceByIdAsync(dto.id);
            if (invoice == null) { return null; }

            invoice.Date = dto.Date;

            await _appDbContext.SaveChangesAsync();
            return new InvoiceDTO
            {
                Id = invoice.InvoiceId,
                Date = invoice.Date,
                Total = invoice.Total,
                OrderId = invoice.OrderId,
            };
        }

        public async Task <InvoiceDTO?> UpdateInvoiceTotalAsync(UpdateInvoiceTotalDTO dto)
        {
            var invoice = await GetInvoiceByIdAsync(dto.id);
            if (invoice == null) { return null; }
            
            invoice.Total = dto.Total;
            await _appDbContext.SaveChangesAsync();
            return new InvoiceDTO
            {
                Id = invoice.InvoiceId,
                Date = invoice.Date,
                Total = invoice.Total,
                OrderId = invoice.OrderId,
            };
        }

        public async Task<bool> DeleteIvnoiceAsync(int id)
        {
            var invoice = await GetInvoiceByIdAsync(id);
            if (invoice == null) { return false; }

            _appDbContext.invoices.Remove(invoice);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
