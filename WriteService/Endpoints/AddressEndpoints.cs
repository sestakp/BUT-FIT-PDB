using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class AddressEndpoints
    {
      
        public static void MapAddressEndpoints(this WebApplication app)
        {
            string path = "/api/address/";

            app.MapPost(path, (AddressService service, AddressDto address) =>
            {
                var newAddress = service.CreateOrUpdate(address);

                if (newAddress.Id != default)
                {
                    return Results.Created(path + newAddress.Id, newAddress);
                }

                return Results.BadRequest();
            });

            app.MapPut(path, (AddressService service, AddressDto address) =>
            {
                var newAddress = service.CreateOrUpdate(address);

                if (newAddress.Id != default)
                {
                    return Results.Ok();
                }

                return Results.BadRequest();
            });

            app.MapDelete(path + "{id:long}", (AddressService service, long id) =>
                service.SoftDelete(id) ? Results.Ok() : Results.NotFound());
        }
    }
}
