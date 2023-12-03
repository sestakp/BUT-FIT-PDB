using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record ProductsOfSubCategory
{
    [BsonId]
    private ObjectId Id { get; init; }

    [BsonElement("category")]
    public required string CategoryName { get; init; }

    [BsonElement("categoryNormalizedName")]
    public required string CategoryNormalizedName { get; set; }

    [BsonElement("subcategory")]
    public required string SubCategoryName { get; init; }

    [BsonElement("subcategoryNormalizedName")]
    public required string SubCategoryNormalizedName { get; init; }

    [BsonElement("products")]
    public List<Product> Products { get; set; } = new();

    public sealed record Product
    {
        [BsonId]
        public long Id { get; set; }

        [BsonElement("title")]
        public required string Title { get; set; }

        [BsonElement("description")]
        public required string Description { get; set; }

        [BsonElement("price")]
        public required decimal Price { get; set; }

        [BsonElement("rating")]
        public required double? Rating { get; set; }

        [BsonElement("vendor")]
        public required ProductVendor Vendor { get; set; }

        public sealed record ProductVendor
        {
            [BsonElement("_id")]
            public long Id { get; set; }

            [BsonElement("name")]
            public required string Name { get; set; }
        }
    }
}