using AutoMapper;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using WriteService.Endpoints;
using WriteService.Services;

namespace WriteService;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        {
            var services = builder.Services;

            services.AddSwaggerGen();

            // TODO: add RabbitMQ configuration

            builder.Configuration.AddUserSecrets<Program>();
            builder.AddRabbitMQSettings();
            builder.AddConnectionFactoryForRabbit();
            builder.AddRabbitConnection();
            builder.AddRabbitChannel();
            builder.AddRabbitMQProducer();

            services.AddCors(options =>
            {
                options.AddPolicy("WriteServiceCorsPolicy", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "DELETE");
                });
            });

            services.AddScoped<CustomerService>();
            services.AddScoped<OrderService>();
            services.AddScoped<ProductService>();
            services.AddScoped<VendorService>();
            services.AddEndpointsApiExplorer();

            services.AddDbContext<ShopDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            services.AddHostedService<ProductGarbageCollector>();

            // TODO: add mapping configurations
            var mapperConfig = new MapperConfiguration(cfg => { });
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        var app = builder.Build();
        {
            app.UseCors("WriteServiceCorsPolicy");
            app.UseHttpsRedirection();
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI();
#endif
            app.MapVendorEndpoints();
            app.MapProductEndpoints();
            app.MapOrderEndpoints();
            app.MapCustomerEndpoints();
        }

        app.Run();
    }
}