using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class VendorEndpoints
    {
        public static void MapVendorEndpoints(this WebApplication app)
        {
            string path = "/api/vendors/";

            app.MapPost(path, (VendorService service, VendorDto vendor) =>
            {
                return "Hello from MyController!";
            });

            app.MapPut(path, (VendorService service, VendorDto vendor) =>
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
