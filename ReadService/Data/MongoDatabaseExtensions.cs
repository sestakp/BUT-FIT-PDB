using MongoDB.Driver;

namespace ReadService.Data;

public static class MongoDatabaseExtensions
{
    public static IMongoCollection<T> Collection<T>(this IMongoDatabase database) =>
        database.GetCollection<T>(typeof(T).Name);
}