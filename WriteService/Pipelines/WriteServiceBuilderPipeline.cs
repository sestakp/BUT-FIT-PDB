using Common.Pipelines;
using Microsoft.EntityFrameworkCore;

namespace WriteService.Pipelines
{
    public class WriteServiceBuilderPipeline : BuilderPipeline
    {

        public static WebApplicationBuilder CreateBuilder(string[] args)
        {

            var builder = BuilderPipeline.CreateBuilder(args, "WriteService");

            // var connectionString = builder.Configuration.GetConnectionString("ShopDbContext");
            string host = "postgres"; // Use the service name as the host
            string database = "cqrs";
            string username = "root";
            string password = "toor";
            string port = "5432"; // PostgreSQL default port

            string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";

            builder.Services.AddDbContext<ShopDbContext>(options =>
                options.UseNpgsql(connectionString));

            return builder;
        }
    }
}
