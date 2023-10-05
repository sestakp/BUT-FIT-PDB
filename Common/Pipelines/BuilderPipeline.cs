using System.Text;
using Common.Extensions;
using Common.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prometheus;

namespace Common.Pipelines
{
    public class BuilderPipeline
    {
        public static WebApplicationBuilder CreateBuilder(string[] args, string serviceName) 
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddSpecificCors();


            builder.AddSwagger(serviceName);

            builder.AddDatabaseSettings();

            builder.AddRabbitMQSettings();

            builder.AddConnectionFactoryForRabbit();
            builder.AddRabbitConnection();
            builder.AddRabbitChannel();


            builder.AddRabbitMQProducer();

            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            });
            

            return builder;
        }
    }
}
