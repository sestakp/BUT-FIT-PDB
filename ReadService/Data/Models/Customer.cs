using MongoDB.Bson.Serialization.Attributes;

namespace ReadService.Data.Models;

public sealed record Customer
{
    [BsonId]
    public required string Email { get; set; }

    [BsonElement("firstName")]
    public required string FirstName { get; set; }

    [BsonElement("lastName")]
    public required string LastName { get; set; }

    [BsonElement("phoneNumber")]
    public required string PhoneNumber { get; set; }

    [BsonElement("addresses")]
    public List<Address> Addresses { get; set; } = new();
}
