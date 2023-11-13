using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTO.Product;
using WriteService.DTO.Review;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/products");

        gb.MapPost(string.Empty, CreateProduct);
        gb.MapPost("{productId:long}/reviews", AddReview);
        gb.MapDelete("{productId:long}", DeleteProduct);
    }

    private static IResult CreateProduct(
        [FromBody] CreateProductDto dto,
        [FromServices] ProductService productService,
        [FromServices] IMapper mapper)
    {
        var product = productService.Create(dto);
        var responseDto = mapper.Map<ProductDto>(product);

        // TODO: add uri from query service
        return Results.Created("todo", responseDto);
    }

    private static IResult AddReview(
        [FromRoute] long productId,
        [FromBody] CreateReviewDto dto,
        [FromServices] ProductService productService,
        [FromServices] IMapper mapper)
    {
        var review = productService.AddReview(productId, dto);
        var responseDto = mapper.Map<ReviewDto>(review);

        // TODO: add uri from query service
        return Results.Created("todo", responseDto);
    }

    private static IResult DeleteProduct(
        [FromRoute] long productId,
        [FromServices] ProductService productService)
    {
        productService.Delete(productId);
        return Results.Ok();
    }
}