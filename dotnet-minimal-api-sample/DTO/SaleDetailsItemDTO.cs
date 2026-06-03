namespace dotnet_minimal_api_sample.DTO
{
    public class SaleDetailsItemDTO
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }

        public decimal LineGrossValue => Quantity * UnitPrice;
        public decimal LineNetValue => (Quantity * UnitPrice) - Discount;
    }
}
