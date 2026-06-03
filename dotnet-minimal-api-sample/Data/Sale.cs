
namespace dotnet_minimal_api_sample.Data
{
    public class Sale
    {
        public string CustomerDocumentNumber { get; set; }
        public string Observations { get; set; }
        public IEnumerable<SaleItem> Products { get; set; }
    }
}
