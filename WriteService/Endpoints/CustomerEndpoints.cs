using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class CustomerEndpoints
    {


        public static void MapCustomerEndpoints(this WebApplication app)
        {
            string path = "/api/customers/";

            app.MapPost(path, (CustomerService service, CustomerDto vendor) =>
            {
                return "Hello from MyController!";
            });

            app.MapPut(path, (CustomerService service, CustomerDto vendor) =>
            {
                return "Hello from MyController!";
            });

            app.MapDelete(path + "{id:long}", (CustomerService service, long id) =>
            {
                return "Hello from MyController!";
            });
        }
    }
}
