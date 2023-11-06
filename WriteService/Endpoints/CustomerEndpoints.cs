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

            app.MapPost(path, (CustomerService service, CustomerDto customer) =>
            {
                var newCustomer = service.CreateOrUpdate(customer);

                if (newCustomer.Id != default)
                {
                    return Results.Created(path + newCustomer.Id, newCustomer);
                }

                return Results.BadRequest();
            });

            app.MapPut(path, (CustomerService service, CustomerDto customer) =>
            {
                var newCustomer = service.CreateOrUpdate(customer);

                if (newCustomer.Id != default)
                {
                    return Results.Ok();
                }

                return Results.BadRequest();
            });

            app.MapDelete(path + "{id:long}", (CustomerService service, long id) =>
            {
                var newCustomer = service.Anonymize(id);

                if (newCustomer.Id != default)
                {
                    return Results.Ok();
                }

                return Results.BadRequest();
            });
        }
    }
}
