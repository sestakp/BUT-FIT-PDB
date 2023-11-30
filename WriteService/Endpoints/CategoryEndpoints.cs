using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/categories");

        gb.MapPost(string.Empty, CreateCategoryAsync);
        gb.MapPut("{customerId:long}", UpdateCustomerAsync);
        gb.MapDelete("{customerId:long}", DeleteCustomerAsync);
        gb.MapPost("{customerId:long}/addresses", CreateCustomerAddress);
        gb.MapPut("{customerId:long}/addresses/{addressId:long}", UpdateCustomerAddressAsync);
        gb.MapDelete("{customerId:long}/addresses/{addressId:long}", DeleteCustomerAddressAsync);
    }

    private static async Task<IResult> CreateCategoryAsync(
        [FromBody] CreateCategoryDto dto,
        [FromServices] CategoryService service,
        [FromServices] IMapper mapper)
    {
        var customer = await service.CreateAsync(dto);
        var responseDto = mapper.Map<CategoryDto>(customer);
        return Results.Created("api/customers" + customer.Email, responseDto);
    }
}
