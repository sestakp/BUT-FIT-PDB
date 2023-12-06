using AutoMapper;
using Common.Configuration;
using Common.Extensions;
using Common.RabbitMQ.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WriteService.DTOs.Address;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.DTOs.Order;
using WriteService.DTOs.Product;
using WriteService.DTOs.Review;
using WriteService.DTOs.SubCategory;
using WriteService.DTOs.Vendor;
using WriteService.Endpoints;
using WriteService.Entities;
using WriteService.Services;

namespace WriteService;

public class Program
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
            services.AddScoped<CategoryService>();
            services.AddScoped<SubCategoryService>();

            services.AddDbContext<ShopDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ShopDbContext")));

            // services.AddHostedService<ProductGarbageCollector>();

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

                cfg.CreateMap<ProductCategoryEntity, CategoryDto>();
                cfg.CreateMap<CategoryDto, ProductCategoryEntity>();

                cfg.CreateMap<ProductSubCategoryEntity, SubCategoryDto>();
                cfg.CreateMap<SubCategoryDto, ProductSubCategoryEntity>();
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

            // app.MapCategoryEndpoints();
            // app.MapSubCategoryEndpoints();
        }

        Console.WriteLine("CONFIGURATION:");
        var rabbitMqConfiguration = app.Services.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;
        var databaseConfiguration = app.Configuration.GetConnectionString("ShopDbContext");
        Console.WriteLine(rabbitMqConfiguration);
        Console.WriteLine(databaseConfiguration);

        UpdateDatabase(app);

        if (args.Length > 0 && args[0] == "--seed")
        {
            SeedDatabase(app);
        }

        app.Run();
    }

    private static void UpdateDatabase(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
            db.Database.Migrate();
        }
    }

    private static void SeedDatabase(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

            // Do not apply database seeds if database is not empty
            if (dbContext.Categories.Any())
            {
                return;
            }

            Seeds.ApplyDatabaseSeeds(dbContext);
            dbContext.SaveChanges();
        }
    }
}