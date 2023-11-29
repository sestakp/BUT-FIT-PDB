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
        app.MapGet("api/customers/{email}", GetCustomerDetail);
        app.MapGet("api/vendors", GetVendors);
        app.MapGet("api/vendors/{id:long}", GetVendorDetail);
        app.MapGet("api/products", GetProducts);
        app.MapGet("api/products/{category}", GetProductsByCategory);
        app.MapGet("api/products/{subcategory}", GetProductsBySubcategory);
        app.MapGet("api/products/{id:long}", GetProductDetail);
        app.MapGet("api/products/{id:long}/reviews", GetProductReviews);
        app.MapGet("api/orders", GetOrdersForCustomer);
        app.MapGet("api/orders/{id:long}", GetOrderDetail);
        app.MapGet("api/categories", GetCategories);
    }

    private static IResult GetCustomers([FromServices] IMongoDatabase database)
    {
        var data = database.Collection<Customer>().AsQueryable();
        return Results.Ok(data);
    }

    private static Task GetCustomerDetail([FromRoute] string email)
    {
        throw new NotImplementedException();
    }

    private static Task GetVendors()
    {
        throw new NotImplementedException();
    }

    private static Task GetVendorDetail([FromRoute] long id)
    {
        throw new NotImplementedException();
    }

    private static Task GetProducts()
    {
        throw new NotImplementedException();
    }

    private static Task GetProductsByCategory([FromRoute] string category)
    {
        throw new NotImplementedException();
    }

    private static Task GetProductsBySubcategory([FromRoute] string subcategory)
    {
        throw new NotImplementedException();
    }

    private static Task GetProductDetail([FromRoute] long id)
    {
        throw new NotImplementedException();
    }

    private static Task GetProductReviews([FromRoute] long id)
    {
        throw new NotImplementedException();
    }

    private static Task GetOrdersForCustomer([FromQuery] string customerEmail)
    {
        throw new NotImplementedException();
    }

    private static Task GetOrderDetail([FromRoute] long id)
    {
        throw new NotImplementedException();
    }

    private static Task GetCategories()
    {
        throw new NotImplementedException();
    }
}