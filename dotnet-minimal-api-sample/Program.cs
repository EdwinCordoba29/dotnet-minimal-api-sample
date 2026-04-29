using dotnet_minimal_api_sample;
using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;
using dotnet_minimal_api_sample.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Linq.Expressions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var ConnectionString = new ConnectionFactory(builder.Configuration.GetConnectionString("SQL"));
builder.Services.AddSingleton(ConnectionString);
builder.Services.AddScoped<IServiceProducts, ServiceProduct>();
builder.Services.AddScoped<IServiceAuth, ServiceAuth>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:ClaveSecreta"]))
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinimalAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autorizacion JWT esquema. \r\n\r\n Escribe 'Bearer' [espacio] y escribe el token proporcionado. \r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { 
            new OpenApiSecurityScheme
            { 
                Reference = new OpenApiReference
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
builder.Services.AddAuthorization();

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
}).RequireAuthorization();

app.MapGet("/api/product/{code}", async (string code, IServiceProducts serviceProducts) =>
{
    Product product = await serviceProducts.GetProduct(code);
    if (product is not null)
    {
        return Results.Ok(product.ConvertDTO());
    }
    return Results.NotFound("El producto no existe.");
}).RequireAuthorization();

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
).RequireAuthorization();

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
).RequireAuthorization();

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
).RequireAuthorization();

app.MapPost("/api/login", async ([FromBody] UserDTO userDTO, IServiceAuth serviceAuth) =>
{
    string token = await serviceAuth.Login(userDTO);
    if (token == String.Empty)
    { 
        return Results.NotFound("Usuario no existe.");
    }
    else
    {
        return Results.Ok(token);
    }
}
).AllowAnonymous();

app.UseHttpsRedirection();

app.Run();

