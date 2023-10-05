using Common.Pipelines;
using Microsoft.EntityFrameworkCore;

namespace WriteService.Pipelines
{
    public class WriteServiceBuilderPipeline : BuilderPipeline
    {

        public static WebApplicationBuilder CreateBuilder(string[] args)
        {

            var builder = BuilderPipeline.CreateBuilder(args, "WriteService");

            var connectionString = builder.Configuration.GetConnectionString("ShopDbContext");

            builder.Services.AddDbContext<ShopDbContext>(options =>
                options.UseNpgsql(connectionString));

            return builder;
        }
    }
}
