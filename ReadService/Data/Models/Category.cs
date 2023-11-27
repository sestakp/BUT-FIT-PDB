using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Category
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("categoryName")]
    public required string CategoryName { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }

    [BsonElement("subCategories")]
    public List<SubCategory> SubCategories { get; set; } = new();
}

public sealed record SubCategory
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }
}