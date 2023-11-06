using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class VendorEndpoints
    {
        public static void MapVendorEndpoints(this WebApplication app)
        {
            const string path = "/api/vendors/";

            app.MapPost(path, (VendorService service, VendorDto vendor) =>
            {
                var newVendor = service.CreateOrUpdate(vendor);

                if (newVendor.Id != default)
                {
                    return Results.Created(path + newVendor.Id, newVendor);
                }
                
                return Results.BadRequest();
                
            });

            app.MapPut(path, (VendorService service, VendorDto vendor) =>
            {
                var newVendor = service.CreateOrUpdate(vendor);

                if (newVendor.Id != default)
                {
                    return Results.Ok();
                }
                
                return Results.BadRequest();
                
            });

            app.MapDelete(path + "{id:long}", (VendorService service, long id) => 
                service.SoftDelete(id) ? Results.Ok() : Results.NotFound());
        }
    }
}
