﻿using Microsoft.AspNetCore.Mvc;
using WriteService.DTOs.Order;
using WriteService.Entities;
using WriteService.Services;

namespace WriteService.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var gb = app.MapGroup("api/orders");

        gb.MapPost(string.Empty, CreateOrderAsync);
        gb.MapPost("{orderId:long}/add-to-cart", AddProductToCartAsync);
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
        [FromBody] AddProductDto dto,
        [FromServices] OrderService orderService)
    {
        var order = await orderService.AddToCartAsync(orderId, dto.ProductId);
        var response = CreateResponse(order);
        return Results.Ok(response);
    }

    private static async Task<IResult> CompleteOrderAsync(
        [FromRoute] long orderId,
        [FromBody] CompleteOrderDto dto,
        [FromServices] OrderService orderService)
    {
        var order = await orderService.CompleteOrderAsync(orderId, dto);
        var response = CreateResponse(order);
        return Results.Ok(response);
    }

    private static object CreateResponse(OrderEntity order)
    {
        return new
        {
            Id = order.Id,
            Status = order.Status,
            DateTimeCreated = order.Created,
            DateTimeLastUpdate = order.LastUpdated,
            Products = order
                .OrderProducts
                .Select(x => new
                {
                    Id = x.Product.Id,
                    Title = x.Product.Title,
                    Price = x.Product.Price,
                    Count = x.Count
                })
                .ToList()
        };
    }
}