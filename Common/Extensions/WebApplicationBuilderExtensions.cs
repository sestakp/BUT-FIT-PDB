using Common.Configuration;
using Common.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Common.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddRabbitMQSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RabbitMQConfiguration>()
            .Bind(configuration.GetSection("RabbitMQ"))
            .ValidateOnStart()
            .ValidateDataAnnotations();
    }

    public static void AddRabbitMQProducer(this IServiceCollection services)
    {
        services.AddScoped<RabbitMQProducer>();
    }

    public static void AddConnectionFactoryForRabbit(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var rabbitMqOptions = sp.GetRequiredService<IOptions<RabbitMQConfiguration>>().Value;

            return new ConnectionFactory
            {
                HostName = rabbitMqOptions.Hostname,
                Port = rabbitMqOptions.Port,
                UserName = rabbitMqOptions.UserName,
                Password = rabbitMqOptions.Password
            };
        });
    }

    public static void AddRabbitConnection(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var connectionFactory = sp.GetRequiredService<ConnectionFactory>();
            return connectionFactory.CreateConnection();
        });
    }

    public static void AddRabbitChannel(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            return connection.CreateModel();
        });
    }
}