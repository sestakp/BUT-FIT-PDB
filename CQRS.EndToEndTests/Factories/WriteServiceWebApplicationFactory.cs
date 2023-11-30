using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using ReadService.Data;
using WriteService;
using WriteService.DTOs.Address;
using WriteService.DTOs.Customer;
using WriteService.DTOs.Order;
using WriteService.DTOs.Product;
using WriteService.DTOs.Review;
using WriteService.DTOs.Vendor;
using WriteService.Entities;
using WriteService.Services;

namespace CQRS.EndToEndTests.Factories;
public class WriteServiceWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {

        builder.ConfigureAppConfiguration((context, config) =>
        {

            var relativePath = "..\\..\\..\\..\\WriteService\\appsettings.Development.json";
            var currentDirectory = Directory.GetCurrentDirectory();
            var fullPath = Path.Combine(currentDirectory, relativePath);
            config.AddJsonFile(fullPath);
        });


        builder.ConfigureServices((context,services) =>
        {

            var configuration = context.Configuration;

            // Configure SQL (Entity Framework Core)
            services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseInMemoryDatabase("CqrsPdbInMemoryForTesting"); // Use an in-memory database for testing
            });

            services.AddSwaggerGen();
            services.AddEndpointsApiExplorer();

            // RabbitMQ configuration
            services.AddRabbitMQSettings(configuration);
            services.AddConnectionFactoryForRabbit();
            services.AddRabbitConnection();
            services.AddRabbitChannel();
            services.AddRabbitMQProducer();
            

            services.AddScoped<CustomerService>();
            services.AddScoped<OrderService>();
            services.AddScoped<ProductService>();
            services.AddScoped<VendorService>();
            

            services.AddHostedService<ProductGarbageCollector>();

            // TODO: refactor to one extension method
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VendorEntity, VendorDto>();
                cfg.CreateMap<VendorDto, VendorEntity>();

                cfg.CreateMap<AddressEntity, AddressDto>();
                cfg.CreateMap<AddressDto, AddressEntity>();

                cfg.CreateMap<CustomerEntity, CustomerDto>();
                cfg.CreateMap<CustomerDto, CustomerEntity>();

                cfg.CreateMap<OrderEntity, CompleteOrderDto>();
                cfg.CreateMap<CompleteOrderDto, OrderEntity>();

                cfg.CreateMap<ProductEntity, ProductDto>();
                cfg.CreateMap<ProductDto, ProductEntity>();

                cfg.CreateMap<ReviewEntity, ReviewDto>();
                cfg.CreateMap<ReviewDto, ReviewEntity>();
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Additional service configurations for testing
        });

        // You can add any other configurations for your application, such as configuration files, here
    }
}
