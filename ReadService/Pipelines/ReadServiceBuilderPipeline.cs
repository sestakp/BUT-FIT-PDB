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


            var connectionString = builder.Configuration.GetConnectionString("MongoDB");

            if (connectionString == null)
            {
                throw new MongoConfigurationException("Connection string not found in user secrets");
            }

            builder.AddMongoClient(connectionString);

            builder.AddMongoDatabase("PDB_CQRS");

            //builder.AddMongoCollection<CollectionName>();

            return builder;
        }
    }
}
