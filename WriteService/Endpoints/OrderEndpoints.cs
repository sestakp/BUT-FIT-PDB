using Microsoft.AspNetCore.Mvc;
using WriteService.DTO.Order;
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

    private static IResult CreateOrder([FromServices] OrderService orderService)
    {
        var order = orderService.Create();
        return Results.Ok(new { id = order.Id });
    }

    private static IResult AddProductToCart(
        [FromRoute] long orderId,
        [FromRoute] long productId,
        [FromServices] OrderService orderService)
    {
        orderService.AddToCart(orderId, productId);
        
        // TODO: return order with products for demonstration purposes?
        return Results.Ok();
    }

    private static IResult CompleteOrder(
        [FromRoute] long orderId,
        [FromBody] CompleteOrderDto dto,
        [FromServices] OrderService orderService)
    {
        orderService.CompleteOrder(orderId, dto);
        
        // TODO: return order with products for demonstration purposes?
        return Results.Ok();
    }
}