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
        app.MapGet("api/customers/{customerId}", GetCustomerDetail);
        app.MapGet("api/vendors", GetVendors);
        app.MapGet("api/vendors/{id:long}", GetVendorDetail);
        app.MapGet("api/products", GetProducts);
        app.MapGet("api/products/{category}", GetProductsByCategory);
        app.MapGet("api/products/{category}/{subcategory}", GetProductsBySubcategory);
        app.MapGet("api/products/{id:long}", GetProductDetail);
        app.MapGet("api/products/{id:long}/reviews", GetProductReviews);
        app.MapGet("api/orders", GetOrdersForCustomer);
        app.MapGet("api/orders/{id:long}", GetOrderDetail);
        app.MapGet("api/categories", GetCategories);
    }

    private static IResult GetCustomers(
        [FromServices] IMongoDatabase database)
    {
        var customers = database
            .Collection<Customer>()
            .AsQueryable();

        return Results.Ok(customers);
    }

    private static IResult GetCustomerDetail(
        [FromRoute] long customerId,
        [FromServices] IMongoDatabase database)
    {
        var customer = database
            .Collection<Customer>()
            .Find(x => x.Id == customerId)
            .SingleOrDefault();

        if (customer is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(customer);
    }

    private static IResult GetVendors(
        [FromServices] IMongoDatabase database)
    {
        var vendors = database
            .Collection<Vendor>()
            .AsQueryable();

        return Results.Ok(vendors);
    }

    private static IResult GetVendorDetail(
        [FromRoute] long id,
        [FromServices] IMongoDatabase database)
    {
        var vendor = database
            .Collection<Vendor>()
            .Find(x => x.Id == id)
            .Single();

        if (vendor is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(vendor);
    }

    // TODO: add pagination
    private static IResult GetProducts(
        [FromServices] IMongoDatabase database)
    {
        var products = database
            .Collection<Product>()
            .AsQueryable();

        return Results.Ok(products);
    }

    private static IResult GetProductsByCategory(
        [FromRoute] string category,
        [FromServices] IMongoDatabase database)
    {
        var products = database
            .Collection<ProductsOfCategory>()
            .Find(x => x.CategoryNameNormalized == category)
            .ToList();

        return Results.Ok(products);
    }

    private static IResult GetProductsBySubcategory(
        [FromRoute] string category,
        [FromRoute] string subcategory,
        [FromServices] IMongoDatabase database)
    {
        var products = database
            .Collection<ProductsOfSubCategory>()
            .Find(x => x.CategoryNameNormalized == category && x.SubCategoryNameNormalized == subcategory)
            .ToList();

        return Results.Ok(products);
    }

    private static IResult GetProductDetail(
        [FromRoute] long id,
        [FromServices] IMongoDatabase database)
    {
        var product = database
            .Collection<Product>()
            .Find(x => x.Id == id)
            .Single();

        if (product is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(product);
    }

    // TODO: add pagination
    private static IResult GetProductReviews(
        [FromRoute] long id,
        [FromServices] IMongoDatabase database)
    {
        var reviews = database
            .Collection<Review>()
            .Find(x => x.ProductId == id)
            .ToList();

        return Results.Ok(reviews);
    }

    private static IResult GetOrdersForCustomer(
        [FromQuery] long customerId,
        [FromServices] IMongoDatabase database)
    {
        var products = database
            .Collection<Order>()
            .Find(x => x.CustomerId == customerId)
            .ToList();

        return Results.Ok(products);
    }

    private static IResult GetOrderDetail(
        [FromRoute] long id,
        [FromServices] IMongoDatabase database)
    {
        var order = database
            .Collection<Order>()
            .Find(x => x.Id == id)
            .Single();

        if (order is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(order);
    }

    private static IResult GetCategories(
        [FromServices] IMongoDatabase database)
    {
        var categories = database
            .Collection<Category>()
            .AsQueryable();

        return Results.Ok(categories);
    }
}