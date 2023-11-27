using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Order
{
    [BsonId]
    public int Id { get; init; }

    [BsonElement("customerId")]
    public required int CustomerId { get; init; }

    [BsonElement("status")]
    public required string Status { get; init; }

    [BsonElement("price")]
    public required decimal Price { get; init; }

    [BsonElement("address")]
    public required Address Address { get; init; }

    [BsonElement("created")]
    public required DateTime Created { get; init; }

    [BsonElement("updated")]
    public required DateTime Updated { get; init; }

    [BsonElement("isDeleted")]
    public required bool IsDeleted { get; init; }

    [BsonElement("products")]
    public required List<Product> Products { get; init; }
}

public sealed record OrderProduct
{
    [BsonId]
    public int Id { get; set; }

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
    [BsonId]
    public int Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }
}