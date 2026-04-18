using dotnet_minimal_api_sample.Data;

namespace dotnet_minimal_api_sample.Data
{
    public class Store
    {
        public static List<Product> ProductList = new List<Product>() {
        new Product
        {
            Id = 1,
            Name = "Laptop Lenovo IdeaPad 3",
            Description = "Laptop 15.6 pulgadas, Ryzen 5, 8GB RAM, 512GB SSD",
            Code = "LAP-001",
            Price = 2500.00m,
            Stock = 15,
            State = true,
            CreationDate = new DateTime(2026, 1, 10, 10, 0, 0)
        },
        new Product
        {
            Id = 2,
            Name = "Laptop HP Pavilion 14",
            Description = "Intel Core i5, 8GB RAM, 256GB SSD",
            Code = "LAP-002",
            Price = 2800.00m,
            Stock = 10,
            State = true,
            CreationDate = new DateTime(2026, 1, 12, 11, 30, 0)
        },
        new Product
        {
            Id = 3,
            Name = "Mouse Logitech Inalámbrico",
            Description = "Mouse óptico inalámbrico USB",
            Code = "ACC-001",
            Price = 80.50m,
            Stock = 50,
            State = true,
            CreationDate = new DateTime(2026, 2, 1, 9, 15, 0)
        },
        new Product
        {
            Id = 4,
            Name = "Teclado Mecánico Redragon",
            Description = "Teclado RGB switches azules",
            Code = "ACC-002",
            Price = 220.99m,
            Stock = 20,
            State = true,
            CreationDate = new DateTime(2026, 2, 5, 14, 20, 0)
        },
        new Product
        {
            Id = 5,
            Name = "Monitor Samsung 24 pulgadas",
            Description = "Full HD, 75Hz, HDMI",
            Code = "MON-001",
            Price = 650.00m,
            Stock = 12,
            State = true,
            CreationDate = new DateTime(2026, 2, 10, 16, 45, 0)
        },
        new Product
        {
            Id = 6,
            Name = "Disco SSD Kingston 1TB",
            Description = "SSD SATA 2.5 pulgadas",
            Code = "COM-001",
            Price = 300.00m,
            Stock = 25,
            State = true,
            CreationDate = new DateTime(2026, 2, 15, 8, 10, 0)
        },
        new Product
        {
            Id = 7,
            Name = "Memoria RAM Corsair 16GB",
            Description = "DDR4 3200MHz",
            Code = "COM-002",
            Price = 180.75m,
            Stock = 30,
            State = true,
            CreationDate = new DateTime(2026, 2, 18, 10, 0, 0)
        },
        new Product
        {
            Id = 8,
            Name = "Laptop Asus TUF Gaming",
            Description = "Ryzen 7, 16GB RAM, RTX 3050",
            Code = "LAP-003",
            Price = 4200.00m,
            Stock = 8,
            State = true,
            CreationDate = new DateTime(2026, 3, 1, 13, 25, 0)
        },
        new Product
        {
            Id = 9,
            Name = "Audífonos HyperX Cloud II",
            Description = "Headset gamer con sonido envolvente",
            Code = "ACC-003",
            Price = 350.40m,
            Stock = 18,
            State = true,
            CreationDate = new DateTime(2026, 3, 5, 15, 0, 0)
        },
        new Product
        {
            Id = 10,
            Name = "Webcam Logitech HD 1080p",
            Description = "Cámara web Full HD con micrófono",
            Code = "ACC-004",
            Price = 210.00m,
            Stock = 22,
            State = true,
            CreationDate = new DateTime(2026, 3, 10, 9, 40, 0)
        }};
    }
}
