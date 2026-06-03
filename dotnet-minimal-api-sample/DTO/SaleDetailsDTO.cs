namespace dotnet_minimal_api_sample.DTO
{
    public class SaleDetailsDTO
    {
        public int SaleId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime SaleDateTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerDocument { get; set; }
        public string? Observations { get; set; }
        public bool State { get; set; }
        public List<SaleDetailsItemDTO> Products { get; set; }

        public decimal TotalGrossValue => Products?.Sum(p => p.LineGrossValue) ?? 0;

        public decimal TotalNetValue => Products?.Sum(p => p.LineNetValue) ?? 0;
    }
}
