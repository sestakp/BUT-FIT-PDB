using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Review
{
    [BsonId]
    public int Id { get; init; }

    [BsonElement("productId")]
    public required int ProductId { get; init; }

    [BsonElement("rating")]
    public required int Rating { get; init; }

    [BsonElement("text")]
    public required string Text { get; init; }
}