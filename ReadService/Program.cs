using Common.Configuration;
using Common.Extensions;
using Common.RabbitMQ;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ReadService.Data;
using ReadService.Data.Models;
using ReadService.Subscribers;

namespace ReadService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();

            // RabbitMQ configuration
            services.AddRabbitMQSettings(configuration);
            services.AddConnectionFactoryForRabbit();
            services.AddRabbitConnection();
            services.AddRabbitChannel();

            services.AddSingleton<CustomerSubscriber>();
            services.AddSingleton<OrderSubscriber>();
            services.AddSingleton<ProductSubscriber>();
            services.AddSingleton<VendorSubscriber>();
            services.AddSingleton<ReviewSubscriber>();

            services.AddCors(options =>
            {
                options.AddPolicy("ReadServiceCorsPolicy", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .WithMethods("GET");
                });
            });

            services.AddMongoDb(configuration);
        }

        var app = builder.Build();
        {
            app.UseCors("ReadServiceCorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI();

            Endpoints.MapEndpoints(app);
        }

        Console.WriteLine("CONFIGURATION:");
        var rabbitMqConfiguration = app.Services.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;
        var databaseConfiguration = app.Services.GetRequiredService<IOptions<MongoDbConfiguration>>().Value;
        Console.WriteLine(rabbitMqConfiguration);
        Console.WriteLine(databaseConfiguration);

        app.Services
            .GetRequiredService<CustomerSubscriber>()
            .ReceiveFromExchange(RabbitMQEntities.Customer);

        app.Services
            .GetRequiredService<OrderSubscriber>()
            .ReceiveFromExchange(RabbitMQEntities.Order);

        app.Services
            .GetRequiredService<ProductSubscriber>()
            .ReceiveFromExchange(RabbitMQEntities.Product);

        app.Services
            .GetRequiredService<VendorSubscriber>()
            .ReceiveFromExchange(RabbitMQEntities.Vendor);

        app.Services
            .GetRequiredService<ReviewSubscriber>()
            .ReceiveFromExchange(RabbitMQEntities.Review);

        if (args.Length > 0 && args[0] == "--seed")
        {
            SeedDatabase(app);
        }

        app.Run();
    }

    private static void SeedDatabase(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            // Do not apply database seeds if database is not empty
            if (database.Collection<Category>().AsQueryable().Any())
            {
                return;
            }

            Seeds.ApplyDatabaseSeeds(database);
        }
    }
}