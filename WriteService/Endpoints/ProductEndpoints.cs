using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Product;
using WriteService.DTOs.Review;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/products");

        gb.MapPost(string.Empty, CreateProductAsync);
        gb.MapPost("{productId:long}/reviews/{customerId}", AddReviewAsync);
        gb.MapDelete("{productId:long}", DeleteProductAsync);
    }

    private static async Task<IResult> CreateProductAsync(
        [FromBody] CreateProductDto dto,
        [FromServices] ProductService productService,
        [FromServices] IMapper mapper)
    {
        var product = await productService.CreateAsync(dto);

        var responseDto = new ProductDto(
            product.Id,
            product.Title,
            product.Description,
            product.Price,
            product.PiecesInStock);

        // TODO: add uri from query service
        return Results.Created("todo", responseDto);
    }

    private static async Task<IResult> AddReviewAsync(
        [FromRoute] long productId,
        [FromRoute] long customerId,
        [FromBody] CreateReviewDto dto,
        [FromServices] ProductService productService,
        [FromServices] IMapper mapper)
    {

        var review = await productService.AddReviewAsync(productId, customerId, dto);
        var responseDto = mapper.Map<ReviewDto>(review);

        // TODO: add uri from query service
        return Results.Created("todo", responseDto);
    }

    private static async Task<IResult> DeleteProductAsync(
        [FromRoute] long productId,
        [FromServices] ProductService productService)
    {
        await productService.DeleteAsync(productId);
        return Results.Ok();
    }
}