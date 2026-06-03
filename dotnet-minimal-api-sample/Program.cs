using dotnet_minimal_api_sample.EndPoint;
using dotnet_minimal_api_sample.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var ConnectionString = new ConnectionFactory(builder.Configuration.GetConnectionString("SQL"));
builder.Services.AddSingleton(ConnectionString);
builder.Services.AddScoped<IServiceProducts, ServiceProduct>();
builder.Services.AddScoped<IServiceCustomers, ServiceCustomer>();
builder.Services.AddScoped<IServiceAuth, ServiceAuth>();
builder.Services.AddScoped<IServiceSales, ServiceSale>();
builder.Host.ConfigureLogging((hostingContext, logging) =>
{
    logging.AddNLog();
});
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

app.ConfigureSalesEndPoints();
app.ConfigureUserEndPoint();
app.ConfigureProductEndPoints();
app.ConfigureCustomerEndPoints();
app.UseHttpsRedirection();

app.Run();

