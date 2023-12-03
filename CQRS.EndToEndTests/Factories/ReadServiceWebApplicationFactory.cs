using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ReadService.Data;
using ReadService.Subscribers;
using WriteService;

namespace CQRS.EndToEndTests.Factories;
public class ReadServiceWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var relativePath = "..\\..\\..\\..\\ReadService\\appsettings.Development.json";
            var currentDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(currentDirectory, relativePath);
            config.AddJsonFile(fullPath);
        });
        

        builder.ConfigureServices((context, services) =>
        {
            
            var configuration = context.Configuration;
            
            
            services.AddMongoDb(configuration,testing:true);
            
            services.AddRabbitMQSettings(configuration);
            services.AddConnectionFactoryForRabbit();
            services.AddRabbitConnection();
            services.AddRabbitChannel();
            
            

            // Additional service configurations for testing
        });

        // You can add any other configurations for your application, such as configuration files, here
    }
}
