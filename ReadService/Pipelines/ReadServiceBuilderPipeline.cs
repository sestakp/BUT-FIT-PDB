using Common.Extensions;
using Common.Pipelines;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ReadService.Pipelines
{
    public class ReadServiceBuilderPipeline : BuilderPipeline
    {
        public static WebApplicationBuilder CreateBuilder(string[] args)
        {

            var builder = BuilderPipeline.CreateBuilder(args, "ReadService");

            string host = "mongodb"; // Use the service name as the host
            int port = 27017; // Default MongoDB port

            string connectionString = $"mongodb://{host}:{port}";

            if (connectionString == null)
            {
                throw new MongoConfigurationException("Connection string not found in user secrets");
            }

            builder.AddMongoClient(connectionString);

            //builder.AddMongoDatabase("PDB_CQRS");

            //builder.AddMongoCollection<CollectionName>();

            return builder;
        }
    }
}
