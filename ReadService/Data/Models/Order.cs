using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Order
{
    [BsonId]
    public long Id { get; init; }

    [BsonElement("customerId")]
    public required string CustomerEmail { get; init; }

    [BsonElement("price")]
    public required decimal Price { get; init; }

    [BsonElement("address")]
    public required OrderAddress Address { get; init; }

    [BsonElement("created")]
    public required DateTime Created { get; init; }

    [BsonElement("isDeleted")]
    public bool IsDeleted { get; init; } = false;

    [BsonElement("products")]
    public required IEnumerable<OrderProduct> Products { get; init; }
}

public sealed record OrderProduct
{
    [BsonElement("title")]
    public required string Title { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }

    [BsonElement("price")]
    public required decimal Price { get; set; }

    [BsonElement("vendor")]
    public required OrderProductVendor Vendor { get; set; }
}

public sealed record OrderProductVendor
{
    [BsonElement("name")]
    public required string Name { get; set; }
}

public sealed record OrderAddress
{
    [BsonElement("country")]
    public required string Country { get; set; }

    [BsonElement("zipCode")]
    public required string ZipCode { get; set; }

    [BsonElement("city")]
    public required string City { get; set; }

    [BsonElement("street")]
    public required string Street { get; set; }

    [BsonElement("houseNumber")]
    public required string HouseNumber { get; set; }
}