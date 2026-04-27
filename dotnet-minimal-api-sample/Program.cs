using dotnet_minimal_api_sample;
using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;
using dotnet_minimal_api_sample.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var ConnectionString = new ConnectionFactory(builder.Configuration.GetConnectionString("SQL"));
builder.Services.AddSingleton(ConnectionString);
builder.Services.AddScoped<IServiceProducts, ServiceProduct>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/products", async (IServiceProducts serviceProducts) =>
{
    var listProducts = (await serviceProducts.GetProducts()).Select(p=>p.ConvertDTO());
    return Results.Ok(listProducts);
});

app.MapGet("/api/product/{code}", async (string code, IServiceProducts serviceProducts) =>
{
    Product product = await serviceProducts.GetProduct(code);
    if (product is not null)
    {
        return Results.Ok(product.ConvertDTO());
    }
    return Results.NotFound("El producto no existe.");
});

app.MapPost("/api/product", async ([FromBody] ProductDTO productDTO, IServiceProducts serviceProducts) =>
{ 

    if(productDTO.Code == String.Empty)
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
    await serviceProducts.CreateProduct(product);
    return Results.Ok();
}
);

app.MapPut("/api/product", async ([FromBody] ProductDTO productDTO, IServiceProducts serviceProducts) =>
{
    if(productDTO.Code == String.Empty)
    {
        return Results.BadRequest("El código no puede ser vacío");
    }

    Product ExistingProduct = await serviceProducts.GetProduct(productDTO.Code);

    if(ExistingProduct != null)
    {
        ExistingProduct.Name = productDTO.Name;
        ExistingProduct.Description = productDTO.Description;
        ExistingProduct.Code = productDTO.Code;
        ExistingProduct.Price = productDTO.Price;
        ExistingProduct.Stock = productDTO.Stock;
        await serviceProducts.UpdateProduct(ExistingProduct);
        return Results.Ok(productDTO);
    }
    else
    {
        return Results.BadRequest("El producto no existe.");
    }
}
);

app.MapDelete("/api/product", async ([FromBody] String Code, IServiceProducts serviceProducts) =>
{
    if (Code == String.Empty)
    {
        return Results.BadRequest("El código no puede ser vacío");
    }

    Product ExistingProduct = await serviceProducts.GetProduct(Code);

    if (ExistingProduct != null)
    {
        await serviceProducts.DeleteProduct(Code);
        return Results.Ok("Producto eliminado correctamente");
    }
    else
    {
        return Results.NotFound("El producto no existe.");
    }
}
);

app.UseHttpsRedirection();

app.Run();

