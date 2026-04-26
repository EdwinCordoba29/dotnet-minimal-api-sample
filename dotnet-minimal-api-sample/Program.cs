using dotnet_minimal_api_sample.Data;
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
    List<Product> listProducts = await serviceProducts.GetProducts();
    return Results.Ok(listProducts);
});

app.MapGet("/api/product/{code}", async (string code, IServiceProducts serviceProducts) =>
{
    Product product = await serviceProducts.GetProduct(code);
    if (product is not null)
    {
        return Results.Ok(product);
    }
    return Results.NotFound("El producto no existe.");
});

app.MapPost("/api/product", async ([FromBody] Product product, IServiceProducts serviceProducts) =>
{ 
    if(product.Code == String.Empty)
    {
        return Results.BadRequest("El código no puede estar vacío");
    }
    await serviceProducts.CreateProduct(product);
    return Results.Ok();
}
);

app.MapPut("/api/product", async ([FromBody] Product product, IServiceProducts serviceProducts) =>
{
    if(product.Code == String.Empty)
    {
        return Results.BadRequest("El código no puede ser vacío");
    }

    Product ExistingProduct = await serviceProducts.GetProduct(product.Code);

    if(ExistingProduct != null)
    {
        product.Id = ExistingProduct.Id;
        await serviceProducts.UpdateProduct(product);
        return Results.Ok(product);
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
        await serviceProducts.DeleteProduct(ExistingProduct.Id);
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

