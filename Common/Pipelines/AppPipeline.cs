using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace Common.Pipelines
{
    public class AppPipeline
    {
        public static WebApplication Build(WebApplicationBuilder builder)
        {
            var app = builder.Build();
            // Configure the HTTP request pipeline.

            app.UseCors("AllowSpecificOrigins");
            app.UseHttpsRedirection();
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI();
#endif

            return app;
        }
    }
}
