using AutoMapper;
using Common.Pipelines;
using WriteService.Endpoints;
using WriteService.Pipelines;
using WriteService.Services;

var builder = WriteServiceBuilderPipeline.CreateBuilder(args);

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<VendorService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHostedService<ProductGarbageCollectorService>();

var mapperConfig = new MapperConfiguration(cfg =>
{
    // TODO
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = AppPipeline.Build(builder);

app.MapVendorEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();
app.MapCustomerEndpoints();

app.Run();