using Common.Pipelines;
using WriteService.Endpoints;
using WriteService.Pipelines;
using WriteService.Services;

var builder = WriteServiceBuilderPipeline.CreateBuilder(args);
builder.Services.AddScoped<VendorService>();
builder.Services.AddEndpointsApiExplorer();
var app = AppPipeline.Build(builder);


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapVendorEndpoints();
app.MapReviewEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();
app.MapCustomerEndpoints();
app.MapAddressEndpoints();
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
