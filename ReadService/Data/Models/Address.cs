using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Address
{
    [BsonId]
    public required long Id { get; set; }

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