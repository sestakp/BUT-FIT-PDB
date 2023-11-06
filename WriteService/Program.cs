using AutoMapper;
using Common.Pipelines;
using WriteService.DTO;
using WriteService.Endpoints;
using WriteService.Entities;
using WriteService.Pipelines;
using WriteService.Services;

var builder = WriteServiceBuilderPipeline.CreateBuilder(args);
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<VendorService>();
builder.Services.AddEndpointsApiExplorer();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<AddressDto, AddressEntity>();
    cfg.CreateMap<CustomerDto, CustomerEntity>();
    cfg.CreateMap<OrderDto, OrderEntity>();
    cfg.CreateMap<ProductDto, ProductEntity>();
    cfg.CreateMap<ReviewDto, ProductReviewEntity>();
    cfg.CreateMap<VendorDto, VendorEntity>();
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = AppPipeline.Build(builder);


app.MapVendorEndpoints();
app.MapReviewEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();
app.MapCustomerEndpoints();
app.MapAddressEndpoints();
app.Run();

