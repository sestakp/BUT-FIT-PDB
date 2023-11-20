using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Address;
using WriteService.DTOs.Customer;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/customers");

        gb.MapPost(string.Empty, CreateCustomer);
        gb.MapPut("{customerId:long}", UpdateCustomer);
        gb.MapDelete("{customerId:long}", DeleteCustomer);
        gb.MapPost("{customerId:long}/addresses", CreateCustomerAddress);
        gb.MapPut("{customerId:long}/addresses/{addressId:long}", UpdateCustomerAddress);
        gb.MapDelete("{customerId:long}/addresses/{addressId:long}", DeleteCustomerAddress);
    }

    private static async Task<IResult> CreateCustomer(
        [FromBody] CreateCustomerDto dto,
        [FromServices] CustomerService service,
        [FromServices] IMapper mapper)
    {
        var customer = await service.CreateAsync(dto);
        var responseDto = mapper.Map<CustomerDto>(customer);
        return Results.Created("api/customers" + customer.Id, responseDto);
    }

    private static async Task<IResult> UpdateCustomer(
        [FromRoute] long customerId,
        [FromBody] UpdateCustomerDto dto,
        [FromServices] CustomerService service,
        [FromServices] IMapper mapper)
    {
        var customer = await service.UpdateAsync(customerId, dto);
        var responseDto = mapper.Map<CustomerDto>(customer);
        return Results.Ok(responseDto);
    }

    private static IResult DeleteCustomer(
        [FromRoute] long customerId,
        [FromServices] CustomerService service)
    {
        service.AnonymizeAsync(customerId);
        return Results.Ok();
    }

    private static async Task<IResult> CreateCustomerAddress(
        [FromBody] CreateAddressDto dto,
        [FromRoute] long customerId,
        [FromServices] CustomerService service,
        [FromServices] IMapper mapper)

    {
        var address = await service.CreateCustomerAddressAsync(customerId, dto);
        var responseDto = mapper.Map<AddressDto>(address);

        // TODO: add uri from query service
        return Results.Created($"api/customers/{customerId}/addresses/{address.Id}", responseDto);
    }

    private static async Task<IResult> UpdateCustomerAddress(
        [FromBody] UpdateAddressDto dto,
        [FromRoute] long customerId,
        [FromRoute] long addressId,
        [FromServices] CustomerService service,
        [FromServices] IMapper mapper)
    {
        var address = await service.UpdateCustomerAddressAsync(customerId, addressId, dto);
        var responseDto = mapper.Map<AddressDto>(address);
        return Results.Ok(responseDto);
    }

    private static IResult DeleteCustomerAddress(
        [FromRoute] long customerId,
        [FromRoute] long addressId,
        [FromServices] CustomerService service)
    {
        service.DeleteCustomerAddressAsync(customerId, addressId);
        return Results.Ok();
    }
}