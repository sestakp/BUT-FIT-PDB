using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Order;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/orders");

        gb.MapPost(string.Empty, CreateOrder);
        gb.MapPost("{orderId:long}/add-to-cart/{productId:long}", AddProductToCart);
        gb.MapPut("{orderId:long}/complete", CompleteOrder);
    }

    private static async Task<IResult> CreateOrder([FromServices] OrderService orderService)
    {
        var order = await orderService.CreateAsync();
        return Results.Ok(new { id = order.Id });
    }

    private static async Task<IResult> AddProductToCart(
        [FromRoute] long orderId,
        [FromRoute] long productId,
        [FromServices] OrderService orderService)
    {
        await orderService.AddToCartAsync(orderId, productId);

        // TODO: return order with products for demonstration purposes?
        return Results.Ok();
    }

    private static async Task<IResult> CompleteOrder(
        [FromRoute] long orderId,
        [FromBody] CompleteOrderDto dto,
        [FromServices] OrderService orderService)
    {
        await orderService.CompleteOrderAsync(orderId, dto);

        // TODO: return order with products for demonstration purposes?
        return Results.Ok();
    }
}