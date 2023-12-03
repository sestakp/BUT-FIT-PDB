using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Category;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/categories");

        gb.MapPost(string.Empty, CreateCategoryAsync);
        gb.MapPut("{categoryId:long}", UpdateCategoryAsync);
        gb.MapDelete("{categoryId:long}", DeleteCategoryAsync);
    }

    private static async Task<IResult> CreateCategoryAsync(
        [FromBody] CreateCategoryDto dto,
        [FromServices] CategoryService service,
        [FromServices] IMapper mapper)
    {
        var category = await service.CreateAsync(dto);
        var responseDto = mapper.Map<CategoryDto>(category);
        return Results.Created("api/categories" + category.Id, responseDto);
    }

    private static async Task<IResult> UpdateCategoryAsync(
        [FromRoute] long categoryId,
        [FromBody] UpdateCategoryDto dto,
        [FromServices] CategoryService service,
        [FromServices] IMapper mapper)
    {
        var customer = await service.UpdateAsync(categoryId, dto);
        var responseDto = mapper.Map<CategoryDto>(customer);
        return Results.Ok(responseDto);
    }

    private static async Task<IResult> DeleteCategoryAsync(
        [FromRoute] long categoryId,
        [FromServices] CategoryService service)
    {
        await service.DeleteAsync(categoryId);
        return Results.Ok();
    }
}