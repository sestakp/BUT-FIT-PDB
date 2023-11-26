using Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Models.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Common.RabbitMQ;
using Microsoft.Net.Http.Headers;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;

namespace Common.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddDatabaseSettings(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("Database"));

            return builder;
        }

        public static WebApplicationBuilder AddRabbitMQSettings(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));

            return builder;
        }

        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder, string name)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = $"{name} API",
                        Description = $"Micro-service for {name} model with CRUD REST API.",
                        Version = "v1"
                    });
            });

            return builder;
        }

        public static WebApplicationBuilder AddMongoClient(this WebApplicationBuilder builder, string connectionString)
        {

            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var databaseSettings = sp.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;

                var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

                return new MongoClient(settings);
            });

            return builder;
        }

        public static WebApplicationBuilder AddMongoDatabase(this WebApplicationBuilder builder, string databaseName)
        {
            builder.Services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            return builder;
        }

        public static WebApplicationBuilder AddMongoCollection<T>(this WebApplicationBuilder builder)
            where T : IMongoItem
        {
            builder.Services.AddScoped(sp =>
            {
                var db = sp.GetRequiredService<IMongoDatabase>();
                return db.GetCollection<T>(
                    sp.GetRequiredService<IOptions<DatabaseConfiguration>>().Value.CollectionName);
            });

            return builder;
        }

        public static WebApplicationBuilder AddRabbitMQProducer(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<RabbitMQProducer>();

            return builder;
        }

        public static WebApplicationBuilder AddConnectionFactoryForRabbit(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                var rabbitMqOptions = sp.GetRequiredService<IOptions<RabbitMQConfiguration>>();
                var hostname = rabbitMqOptions.Value.Hostname;
                var port = rabbitMqOptions.Value.Port;
                var username = rabbitMqOptions.Value.UserName;
                var password = rabbitMqOptions.Value.Password;

#if DEBUG
                hostname = "localhost";
#endif

                return new ConnectionFactory
                {
                    HostName = hostname,
                    Port = port,
                    UserName = username,
                    Password = password
                };
            });
            return builder;
        }

        public static WebApplicationBuilder AddRabbitConnection(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                var connectionFactory = sp.GetRequiredService<ConnectionFactory>();
                return connectionFactory.CreateConnection();
            });

            return builder;
        }

        public static WebApplicationBuilder AddRabbitChannel(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateModel();
            });

            return builder;
        }

        public static WebApplicationBuilder AddSpecificCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("http://build-pool", "http://localhost:5173", "http://172.16.1.1", "http://172.16.1.2")
                        .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)
                        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                        .AllowCredentials();
                });
            });
            return builder;
        }

    }
}
