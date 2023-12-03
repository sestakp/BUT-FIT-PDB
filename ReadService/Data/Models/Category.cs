using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Category
{
    [BsonId]
    private ObjectId Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("normalizedName")]
    public required string NormalizedName { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }

    [BsonElement("subcategories")]
    public List<SubCategory> SubCategories { get; set; } = new();
}

public sealed record SubCategory
{
    [BsonId]
    private ObjectId Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("normalizedName")]
    public required string NormalizedName { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }
}