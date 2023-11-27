using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService;

public sealed class Endpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapGet("api/customers", GetCustomers);
    }

    private static IResult GetCustomers([FromServices] IMongoDatabase database)
    {
        var data = database.Collection<Customer>().AsQueryable();
        return Results.Ok(data);
    }
}