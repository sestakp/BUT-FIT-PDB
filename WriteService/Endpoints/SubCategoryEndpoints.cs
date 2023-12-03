using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.DTOs.SubCategory;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class SubCategoryEndpoints
{
    public static void MapSubCategoryEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/subCategories");

        gb.MapPost(string.Empty, CreateCategoryAsync);
        gb.MapPut("{subCategoryId:long}", UpdateCategoryAsync);
        gb.MapDelete("{subCategoryId:long}", DeleteCategoryAsync);
    }

    private static async Task<IResult> CreateCategoryAsync(
        [FromBody] CreateSubCategoryDto dto,
        [FromServices] SubCategoryService service,
        [FromServices] IMapper mapper)
    {
        var category = await service.CreateAsync(dto);
        var responseDto = mapper.Map<SubCategoryDto>(category);
        return Results.Created("api/subCategories" + category.Id, responseDto);
    }

    private static async Task<IResult> UpdateCategoryAsync(
        [FromRoute] long subCategoryId,
        [FromBody] UpdateSubCategoryDto dto,
        [FromServices] SubCategoryService service,
        [FromServices] IMapper mapper)
    {
        var customer = await service.UpdateAsync(subCategoryId, dto);
        var responseDto = mapper.Map<SubCategoryDto>(customer);
        return Results.Ok(responseDto);
    }

    private static async Task<IResult> DeleteCategoryAsync(
        [FromRoute] long subCategoryId,
        [FromServices] SubCategoryService service)
    {
        await service.DeleteAsync(subCategoryId);
        return Results.Ok();
    }
}
