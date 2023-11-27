using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Vendor
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("address")]
    public required Address Address { get; set; }

    [BsonElement("category")]
    public List<string> Categories { get; set; } = null!;
}