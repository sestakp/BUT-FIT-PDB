using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Review
{
    [BsonId]
    public long Id { get; init; }

    [BsonElement("productId")]
    public required long ProductId { get; init; }

    [BsonElement("rating")]
    public required double Rating { get; init; }

    [BsonElement("text")]
    public required string Text { get; init; }
}