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

            app.MapPost(path, (OrderService service, OrderDto vendor) =>
            {
                return "Hello from MyController!";
            });

            app.MapPost(path+ "addToCart/{orderId}/{productId}", (OrderService service, long productId, long orderId) =>
            {
                return "Hello from MyController!";
            });

            app.MapPut(path + "{id}/pay", (OrderService service, long id) =>
            {
                return "Hello from MyController!";
            });
        }
    }
}
