using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Vendor
{
    [BsonId]
    public long Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("address")]
    public required VendorAddress Address { get; set; }

    [BsonElement("categories")]
    public List<string> Categories { get; init; } = new();
}

public sealed record VendorAddress
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