using System;

namespace dotnet_minimal_api_sample.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }     
        public string? Description { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool State { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public string? UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }

}
}
