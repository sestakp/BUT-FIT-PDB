using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Vendor;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class VendorEndpoints
{
    public static void MapVendorEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/vendors");

        gb.MapPost(string.Empty, CreateVendor);
        gb.MapPut("{vendorId:long}", UpdateVendor);
        gb.MapDelete("{vendorId:long}", DeleteVendor);
    }

    private static async Task<IResult> CreateVendor(
        [FromBody] CreateVendorDto dto,
        [FromServices] VendorService vendorService,
        [FromServices] IMapper mapper)
    {
        var vendor = await vendorService.CreateAsync(dto);
        var responseDto = mapper.Map<VendorDto>(vendor);

        // TODO: add uri from query service
        return Results.Created("todo", responseDto);
    }

    private static async Task<IResult> UpdateVendor(
        [FromRoute] long vendorId,
        [FromBody] UpdateVendorDto dto,
        [FromServices] VendorService vendorService,
        [FromServices] IMapper mapper)
    {
        var vendor = await vendorService.UpdateAsync(vendorId, dto);
        var responseDto = mapper.Map<VendorDto>(vendor);

        // TODO: add uri from query service
        return Results.Created("todo", responseDto);
    }

    private static IResult DeleteVendor(
        [FromRoute] long vendorId,
        [FromServices] VendorService vendorService)
    {
        vendorService.DeleteAsync(vendorId);
        return Results.Ok();
    }
}