using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record ProductsOfSubCategory
{
    // Tuple (string ParentCategory, string SubCategory) can not be used as BsonId because it is serialized to array, and arrays are not supported for ids in MongoDB.
    // For that reason, are have Category and SubCategory as separate properties.
    // As we do not have any explicit BsonId, MongoDb will create it for us and it will be of type ObjectId.

    [BsonElement("category")]
    public required string CategoryName { get; init; }

    [BsonElement("subcategory")]
    public required string SubCategoryName { get; init; }

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