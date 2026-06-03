using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;
using dotnet_minimal_api_sample.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_minimal_api_sample.EndPoint
{
    public static class ProductAPI
    {
        public static void ConfigureProductEndPoints(this WebApplication app)
        {
            app.MapGet("/api/products", GetProducts).RequireAuthorization();
            app.MapGet("/api/product/{code}", GetProduct).RequireAuthorization();
            app.MapPost("/api/product", CreateProduct).RequireAuthorization();
            app.MapPut("/api/product", UpdateProduct).RequireAuthorization();
            app.MapDelete("/api/product/{code}", DeleteProduct).RequireAuthorization();
        }

        private async static Task<IResult> GetProducts(IServiceProducts serviceProducts)
        {
            var listProducts = (await serviceProducts.GetProducts()).Select(p => p.ConvertDTO());
            return Results.Ok(listProducts);
        }

        private async static Task<IResult> GetProduct(string code, IServiceProducts serviceProducts)
        {
            Product product = await serviceProducts.GetProduct(code);
            if (product is not null)
            {
                return Results.Ok(product.ConvertDTO());
            }
            return Results.NotFound("El producto no existe.");
        }

        private async static Task<IResult> CreateProduct([FromBody] ProductDTO productDTO, IServiceProducts serviceProducts, ClaimsPrincipal user) 
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (productDTO.Code == String.Empty)
            {
                return Results.BadRequest("El código no puede estar vacío");
            }
            Product product = null;
            product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Code = productDTO.Code,
                Price = productDTO.Price,
                Stock = productDTO.Stock
            };
            await serviceProducts.CreateProduct(product, userId);
            return Results.Ok();
        }

        private async static Task<IResult> UpdateProduct([FromBody] ProductDTO productDTO, IServiceProducts serviceProducts, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (productDTO.Code == String.Empty)
            {
                return Results.BadRequest("El código no puede ser vacío");
            }

            Product ExistingProduct = await serviceProducts.GetProduct(productDTO.Code);

            if (ExistingProduct != null)
            {
                ExistingProduct.Name = productDTO.Name;
                ExistingProduct.Description = productDTO.Description;
                ExistingProduct.Code = productDTO.Code;
                ExistingProduct.Price = productDTO.Price;
                ExistingProduct.Stock = productDTO.Stock;
                await serviceProducts.UpdateProduct(ExistingProduct, userId);
                return Results.Ok(productDTO);
            }
            else
            {
                return Results.BadRequest("El producto no existe.");
            }
        }

        private async static Task<IResult> DeleteProduct(String code, IServiceProducts serviceProducts, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (code == String.Empty)
            {
                return Results.BadRequest("El código no puede ser vacío");
            }

            Product ExistingProduct = await serviceProducts.GetProduct(code);

            if (ExistingProduct != null)
            {
                await serviceProducts.DeleteProduct(code, userId);
                return Results.Ok("Producto eliminado correctamente");
            }
            else
            {
                return Results.NotFound("El producto no existe.");
            }
        }
    }
}
