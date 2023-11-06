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

            app.MapPost(path, (VendorService service, ProductDto vendor) =>
            {
                return "Hello from MyController!";
            });
            
            app.MapDelete(path + "{id:long}", (VendorService service, long id) =>
            {
                return "Hello from MyController!";
            });
        }
    }
}
