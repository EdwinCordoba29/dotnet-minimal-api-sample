using dotnet_minimal_api_sample.Data;
using dotnet_minimal_api_sample.DTO;
using dotnet_minimal_api_sample.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_minimal_api_sample.EndPoint
{
    public static class CustomerAPI
    {
        public static void ConfigureCustomerEndPoints(this WebApplication app)
        {
            app.MapGet("/api/customers", GetCustomers).RequireAuthorization();
            app.MapGet("/api/customer/{documentNumber}", GetCustomer).RequireAuthorization();
            app.MapPost("/api/customer", CreateCustomer).RequireAuthorization();
            app.MapPut("/api/customer", UpdateCustomer).RequireAuthorization();
            app.MapDelete("/api/customer/{documentNumber}", DeleteCustomer).RequireAuthorization();
        }

        private async static Task<IResult> GetCustomers(IServiceCustomers serviceCustomers)
        {
            var listCustomers = (await serviceCustomers.GetCustomers()).Select(c => c.ConvertDTO());
            return Results.Ok(listCustomers);
        }

        private async static Task<IResult> GetCustomer(string documentNumber, IServiceCustomers serviceCustomers)
        {
            Customer customer = await serviceCustomers.GetCustomer(documentNumber);
            if (customer is not null)
            {
                return Results.Ok(customer.ConvertDTO());
            }
            return Results.NotFound("El cliente no existe.");
        }

        private async static Task<IResult> CreateCustomer([FromBody] CustomerDTO customerDTO, IServiceCustomers serviceCustomers, ClaimsPrincipal user) 
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerDTO.DocumentNumber))
            {
                return Results.BadRequest("El número de documento no puede estar vacío");
            }
            Customer customer = new Customer
            {
                FirstName = customerDTO.FirstName,
                MiddleName = customerDTO.MiddleName,
                LastName = customerDTO.LastName,
                SecondLastName = customerDTO.SecondLastName,
                DocumentType = customerDTO.DocumentType,
                DocumentNumber = customerDTO.DocumentNumber,
                Email = customerDTO.Email,
                Phone = customerDTO.Phone,
                City = customerDTO.City,
                Address = customerDTO.Address
            };
            await serviceCustomers.CreateCustomer(customer, userId);
            return Results.Ok();
        }

        private async static Task<IResult> UpdateCustomer([FromBody] CustomerDTO customerDTO, IServiceCustomers serviceCustomers, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerDTO.DocumentNumber))
            {
                return Results.BadRequest("El número de documento no puede ser vacío");
            }

            Customer existingCustomer = await serviceCustomers.GetCustomer(customerDTO.DocumentNumber);

            if (existingCustomer != null)
            {
                existingCustomer.FirstName = customerDTO.FirstName;
                existingCustomer.MiddleName = customerDTO.MiddleName;
                existingCustomer.LastName = customerDTO.LastName;
                existingCustomer.SecondLastName = customerDTO.SecondLastName;
                existingCustomer.DocumentType = customerDTO.DocumentType;
                existingCustomer.DocumentNumber = customerDTO.DocumentNumber;
                existingCustomer.Email = customerDTO.Email;
                existingCustomer.Phone = customerDTO.Phone;
                existingCustomer.City = customerDTO.City;
                existingCustomer.Address = customerDTO.Address;
                await serviceCustomers.UpdateCustomer(existingCustomer, userId);
                return Results.Ok(customerDTO);
            }
            else
            {
                return Results.BadRequest("El cliente no existe.");
            }
        }

        private async static Task<IResult> DeleteCustomer(string documentNumber, IServiceCustomers serviceCustomers, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(documentNumber))
            {
                return Results.BadRequest("El número de documento no puede ser vacío");
            }

            Customer existingCustomer = await serviceCustomers.GetCustomer(documentNumber);

            if (existingCustomer != null)
            {
                await serviceCustomers.DeleteCustomer(documentNumber, userId);
                return Results.Ok("Cliente eliminado correctamente");
            }
            else
            {
                return Results.NotFound("El cliente no existe.");
            }
        }
    }
}
