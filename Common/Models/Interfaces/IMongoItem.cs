namespace Common.Models.Interfaces
{
    public interface IMongoItem
    {
        // ObjectId as string for testing purposes (Swagger has inconsistency in request and response forms of ObjectId)
        string Id { get; set; }
    }
}
