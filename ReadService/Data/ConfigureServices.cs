using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ReadService.Data;

public static class ConfigureServices
{
    public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoDbOptions>()
            .Bind(configuration.GetSection(nameof(MongoDbOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IMongoDatabase>((sp) =>
        {
            var options = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
            return new MongoClient(options.ConnectionString).GetDatabase(options.DatabaseName);
        });
    }
}