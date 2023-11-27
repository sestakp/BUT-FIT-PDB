using Common.Extensions;
using Common.RabbitMQ;
using ReadService.Data;
using ReadService.Subscribers;

namespace ReadService;

internal static class Program
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
        }

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

        app.Run();
    }
}