using AutoMapper;
using Common.Extensions;
using Common.RabbitMQ.MessageDTOs;
using Microsoft.EntityFrameworkCore;
using WriteService.DTOs.Address;
using WriteService.DTOs.Customer;
using WriteService.DTOs.Order;
using WriteService.DTOs.Product;
using WriteService.DTOs.Review;
using WriteService.DTOs.Vendor;
using WriteService.Endpoints;
using WriteService.Entities;
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
            services.AddEndpointsApiExplorer();

            // RabbitMQ configuration
            services.AddRabbitMQSettings(builder.Configuration);
            services.AddConnectionFactoryForRabbit();
            services.AddRabbitConnection();
            services.AddRabbitChannel();
            services.AddRabbitMQProducer();

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
                options.UseNpgsql(builder.Configuration.GetConnectionString("ShopDbContext")));

            services.AddHostedService<ProductGarbageCollector>();

            // TODO: refactor to one extension method
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<VendorEntity, VendorDto>();
                cfg.CreateMap<VendorDto, VendorEntity>();
                cfg.CreateMap<VendorEntity, VendorMessageDTO>();

                cfg.CreateMap<AddressEntity, AddressDto>();
                cfg.CreateMap<AddressDto, AddressEntity>();
                cfg.CreateMap<AddressEntity, AddressMessageDTO>();

                cfg.CreateMap<CustomerEntity, CustomerDto>();
                cfg.CreateMap<CustomerDto, CustomerEntity>();
                cfg.CreateMap<CustomerEntity, CustomerMessageDTO>();

                cfg.CreateMap<OrderEntity, CompleteOrderDto>();
                cfg.CreateMap<CompleteOrderDto, OrderEntity>();
                cfg.CreateMap<OrderEntity, OrderMessageDTO>();

                cfg.CreateMap<ProductEntity, ProductDto>();
                cfg.CreateMap<ProductDto, ProductEntity>();
                cfg.CreateMap<ProductEntity, ProductMessageDto>();

                cfg.CreateMap<ReviewEntity, ReviewDto>();
                cfg.CreateMap<ReviewDto, ReviewEntity>();
                cfg.CreateMap<ReviewEntity, ReviewMessageDTO>();
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        var app = builder.Build();
        {
            app.UseCors("WriteServiceCorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapVendorEndpoints();
            app.MapProductEndpoints();
            app.MapOrderEndpoints();
            app.MapCustomerEndpoints();
        }

        app.Run();
    }
}