using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;
using dotnet_minimal_api_sample.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_minimal_api_sample.EndPoint
{
    public static class SaleAPI
    {
        public static void ConfigureSalesEndPoints(this WebApplication app)
        {
            app.MapPost("/api/sale", CreateSale).RequireAuthorization();
            app.MapGet("/api/sales", GetAllSales).RequireAuthorization();
            app.MapGet("/api/sales/customer/{documentNumber}", GetSalesByCustomer).RequireAuthorization();
            app.MapGet("/api/sales/invoice/{invoiceNumber}", GetSaleByInvoice).RequireAuthorization();
            app.MapDelete("/api/sales/cancel/{invoiceNumber}", CancelSale).RequireAuthorization();
        }

        private async static Task<IResult> CreateSale([FromBody] SaleDTO saleDTO, IServiceSales serviceSales, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);


            if (saleDTO == null)
            {
                return Results.BadRequest("El cuerpo de la petición no puede ser nulo");
            }

            if (string.IsNullOrEmpty(saleDTO.CustomerDocumentNumber))
            {
                return Results.BadRequest("El número de documento no puede estar vacío");
            }

            if (saleDTO.Products == null || !saleDTO.Products.Any())
            {
                return Results.BadRequest("La venta debe contener al menos un producto");
            }

            if (saleDTO.Products.Any(p => p.Quantity <= 0))
            {
                return Results.BadRequest("La cantidad de cada producto debe ser mayor a cero");
            }

            Sale sale = new Sale
            {
                CustomerDocumentNumber = saleDTO.CustomerDocumentNumber,
                Observations = saleDTO.Observations,
                Products = saleDTO.Products.Select(p => new SaleItem
                {
                    ProductCode = p.ProductCode,
                    Quantity = p.Quantity,
                    Discount = p.Discount
                }
                ).ToList()
            };

            await serviceSales.CreateSale(sale, userId);
            return Results.Ok(new { message = "Venta creada exitosamente" });
        }

        private async static Task<IResult> GetAllSales(IServiceSales serviceSales)
        {
            var sales = await serviceSales.GetSales();
            if (sales is not null)
            {
                return Results.Ok(sales);
            }
            else 
            {
                return Results.Problem("Error al obtener las ventas.");
            }
        }

        private async static Task<IResult> GetSalesByCustomer(string documentNumber, IServiceSales serviceSales)
        {
            if (string.IsNullOrEmpty(documentNumber)) return Results.BadRequest("El documento del cliente es requerido");

            var sales = await serviceSales.GetSalesByCustomer(documentNumber);
            if (sales is not null)
            {
                return Results.Ok(sales);
            }
            else
            {
                return Results.Problem("Error al obtener las ventas del cliente.");
            }
        }

        private async static Task<IResult> GetSaleByInvoice(string invoiceNumber, IServiceSales serviceSales)
        {
            if (string.IsNullOrEmpty(invoiceNumber)) return Results.BadRequest("El número de factura es requerido");

            var sales = await serviceSales.GetSaleByInvoice(invoiceNumber);
            if (sales == null || !sales.Any()) return Results.NotFound("No se encontró ninguna venta con el número de factura proporcionado");

            return Results.Ok(sales);
        }

        private async static Task<IResult> CancelSale(string invoiceNumber, IServiceSales serviceSales, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(invoiceNumber))
                return Results.BadRequest("El número de factura es obligatorio");

            await serviceSales.CancelSale(invoiceNumber, userId);
            return Results.Ok(new { message = $"La factura {invoiceNumber} ha sido cancelada exitosamente" });
        }
    }
}
