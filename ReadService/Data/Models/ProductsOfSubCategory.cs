using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record ProductsOfSubCategory
{
    [BsonId]
    public required (string, string) Id { get; set; }

    [BsonElement("products")]
    public List<Product> Products { get; set; } = new();

    public sealed record Product
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("title")]
        public required string Title { get; set; }

        [BsonElement("description")]
        public required string Description { get; set; }

        [BsonElement("price")]
        public required decimal Price { get; set; }

        [BsonElement("rating")]
        public required int Rating { get; set; }

        [BsonElement("vendor")]
        public required ProductVendor Vendor { get; set; }

        public sealed record ProductVendor
        {
            [BsonElement("_id")]
            public int Id { get; set; }

            [BsonElement("name")]
            public required string Name { get; set; }
        }
    }
}