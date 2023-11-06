using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class OrderEndpoints
    {
        
        public static void MapOrderEndpoints(this WebApplication app)
        {
            string path = "/api/orders/";

            app.MapPost(path, (OrderService service, OrderDto order) =>
            {
                var newOrder = service.Create(order);

                if (newOrder.Id != default)
                {
                    return Results.Created(path + newOrder.Id, newOrder);
                }

                return Results.BadRequest();
            });

            app.MapPost(path+ "addToCart/{orderId}/{productId}", (OrderService service, long orderId, long productId) =>
            {
                var newOrder = service.AddToCart(orderId,productId);

                if (newOrder.Id != default)
                {
                    return Results.Ok();
                }

                return Results.BadRequest();
            });

            app.MapPut(path + "{orderId:long}/pay", (OrderService service, long orderId) =>
            {
                var newOrder = service.Pay(orderId);

                if (newOrder.Id != default)
                {
                    return Results.Ok();
                }

                return Results.BadRequest();
            });
        }
    }
}
