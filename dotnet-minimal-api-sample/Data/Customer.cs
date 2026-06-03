using System;

namespace dotnet_minimal_api_sample.Data
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public bool State { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdatedByUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeletedByUserId { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
