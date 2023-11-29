using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Category
{
    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }

    [BsonElement("subcategories")]
    public List<SubCategory> SubCategories { get; set; } = new();
}

public sealed record SubCategory
{
    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }
}