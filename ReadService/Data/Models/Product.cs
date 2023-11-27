using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Product
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("title")]
    public required string Title { get; set; }

    [BsonElement("description")]
    public required string Description { get; set; }

    [BsonElement("piecesInStock")]
    public required int PiecesInStock { get; set; }

    [BsonElement("price")]
    public required decimal Price { get; set; }

    [BsonElement("rating")]
    public required int Rating { get; set; }

    [BsonElement("vendor")]
    public required ProductVendor Vendor { get; set; }

    [BsonElement("categories")]
    public required List<ProductCategory> Categories { get; set; }

    [BsonElement("subcategories")]
    public required List<ProductSubcategory> Subcategories { get; set; }
}

public sealed record ProductVendor
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }
}

public sealed record ProductCategory
{
    [BsonElement("name")]
    public required string Name { get; set; }
}

public sealed record ProductSubcategory
{
    [BsonElement("name")]
    public required string Name { get; set; }
}