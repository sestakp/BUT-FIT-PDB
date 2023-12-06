using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WriteService;

namespace CQRS.EndToEndTests.Factories;
public class WriteServiceWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {

            var configuration = context.Configuration;

            // Configure SQL (Entity Framework Core)
            services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseInMemoryDatabase("CqrsPdbInMemoryForTesting"); // Use an in-memory database for testing
            });


            // RabbitMQ configuration
            services.AddRabbitMQSettings(configuration);
            services.AddConnectionFactoryForRabbit();
            services.AddRabbitConnection();
            services.AddRabbitChannel();
            services.AddRabbitMQProducer();




            // Additional service configurations for testing
        });

        // You can add any other configurations for your application, such as configuration files, here
    }
}