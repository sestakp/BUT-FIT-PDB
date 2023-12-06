using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using ReadService.Data;

namespace CQRS.EndToEndTests.Factories;
public class ReadServiceWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {

            var configuration = context.Configuration;


            services.AddMongoDb(configuration, testing: true);

            services.AddRabbitMQSettings(configuration);
            services.AddConnectionFactoryForRabbit();
            services.AddRabbitConnection();
            services.AddRabbitChannel();



            // Additional service configurations for testing
        });

        // You can add any other configurations for your application, such as configuration files, here
    }
}