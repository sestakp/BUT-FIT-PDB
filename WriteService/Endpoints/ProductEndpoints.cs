using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class ProductEndpoints
    {
      

        public static void MapProductEndpoints(this WebApplication app)
        {
            string path = "/api/products/";

            app.MapPost(path, (ProductService service, ProductDto product) =>
            {
                var newProduct = service.Create(product);

                if (newProduct.Id != default)
                {
                    return Results.Created(path + newProduct.Id, newProduct);
                }
                return Results.BadRequest();
            });
            
            app.MapDelete(path + "{id:long}", (ProductService service, long id) =>
                service.SoftDelete(id) ? Results.Ok() : Results.NotFound());
        }
    }
}
