﻿using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class AddressEndpoints
    {
      
        public static void MapAddressEndpoints(this WebApplication app)
        {
            string path = "/api/address/";

            app.MapPost(path, (VendorService service, AddressDto vendor) =>
            {
                return "Hello from MyController!";
            });

            app.MapPut(path, (VendorService service, AddressDto vendor) =>
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
