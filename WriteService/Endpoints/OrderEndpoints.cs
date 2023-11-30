using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Order;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/orders");

        gb.MapPost(string.Empty, CreateOrderAsync);
        gb.MapPost("{orderId:long}/add-to-cart/{productId:long}", AddProductToCartAsync);
        gb.MapPut("{orderId:long}/complete", CompleteOrderAsync);
    }

    private static async Task<IResult> CreateOrderAsync(
        [FromBody] CreateOrderDto dto,
        [FromServices] OrderService orderService)
    {
        var order = await orderService.CreateAsync(dto.CustomerId);
        
        return Results.Ok(order.Id);
    }

    private static async Task<IResult> AddProductToCartAsync(
        [FromRoute] long orderId,
        [FromRoute] long productId,
        [FromServices] OrderService orderService)
    {
        await orderService.AddToCartAsync(orderId, productId);

        // TODO: return order with products for demonstration purposes?
        return Results.Ok();
    }

    private static async Task<IResult> CompleteOrderAsync(
        [FromRoute] long orderId,
        [FromBody] CompleteOrderDto dto,
        [FromServices] OrderService orderService)
    {
        await orderService.CompleteOrderAsync(orderId, dto);

        // TODO: return order with products for demonstration purposes?
        return Results.Ok();
    }
}