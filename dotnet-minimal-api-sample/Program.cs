using dotnet_minimal_api_sample.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/products", () =>
{
    return Results.Ok(Store.ProductList);
}
);

app.MapGet("/api/product/{code}", (string code) =>
{
    return Results.Ok(Store.ProductList.FirstOrDefault(p => p.Code == code));
}
);

app.MapPost("/api/product", ([FromBody] Product product) =>
{ 
    if(product.Id < 1 || product.Code == String.Empty)
    {
        return Results.BadRequest("El Id debe ser mayor a 0 y el código no puede estar vacío");
    }
    if(Store.ProductList.Any(p => p.Code.ToLower() == product.Code.ToLower()))
    {
        return Results.BadRequest("El código ya existe");
    }
    product.Id = Store.ProductList.OrderByDescending(p => p.Id).FirstOrDefault().Id + 1;
    Store.ProductList.Add(product);
    return Results.Ok(product);
}
);

app.MapPut("/api/product", ([FromBody] Product product) =>
{
    if(product.Code == String.Empty)
    {
        return Results.BadRequest("El código no puede ser vacío");
    }
    Product ExistingProduct = Store.ProductList.FirstOrDefault(p => p.Code == product.Code);

    if(ExistingProduct != null)
    {
        ExistingProduct.Name = product.Name;
        ExistingProduct.Description = product.Description;
        ExistingProduct.Code = product.Code;
        ExistingProduct.Price = product.Price;
        ExistingProduct.Stock = product.Stock;
        ExistingProduct.State = product.State;
        ExistingProduct.UpdateDate = DateTime.Now;
        Store.ProductList[Store.ProductList.IndexOf(ExistingProduct)] = ExistingProduct;
        return Results.Ok(ExistingProduct);
    }
    else
    {
        return Results.BadRequest("Ocurrió un error al actualizar el producto.");
    }
}
);

app.MapDelete("/api/product", ([FromBody] String Code) =>
{
    if (Code == String.Empty)
    {
        return Results.BadRequest("El código no puede ser vacío");
    }

    Product ExistingProduct = Store.ProductList.Find(p => p.Code == Code);

    if (ExistingProduct != null)
    {
        Store.ProductList.Remove(ExistingProduct);
        return Results.Ok("Producto eliminado correctamente");
    }
    else
    {
        return Results.NotFound("Ocurrió un error al eliminar el producto.");
    }
}
);

app.UseHttpsRedirection();

app.Run();

