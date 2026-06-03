namespace dotnet_minimal_api_sample.DTO
{
    public class SaleDTO
    {
        public string CustomerDocumentNumber { get; set; }
        public string Observations { get; set; }
        public IEnumerable<SaleItemDTO> Products { get; set; }
    }
}
