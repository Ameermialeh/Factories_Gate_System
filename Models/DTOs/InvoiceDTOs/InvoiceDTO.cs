namespace FactoriesGateSystem.Models.DTOs.InvoiceDTOs
{
    public class InvoiceDTO
    {
        public int Id { get; set; }

        public decimal Total { get; set; }

        public DateTime Date { get; set; }

        public int OrderId { get; set; }
    }
}
