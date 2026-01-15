namespace FactoriesGateSystem.DTOs.InvoiceDTOs
{
    public class UpdateInvoiceDTO
    {

        public class UpdateInvoiceDateDTO
        {
            public int id  { get; set; }
            public DateTime Date { get; set; }
        }
        public class UpdateInvoiceTotalDTO
        {
            public int id  { get; set; }
            public decimal Total { get; set; }
        }
    }
}
