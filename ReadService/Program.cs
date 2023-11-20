using Common.Extensions;
using Common.Pipelines;
using Common.RabbitMQ;
using ReadService.Pipelines;
using ReadService.Subscribers;

var builder = ReadServiceBuilderPipeline.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();
builder.AddRabbitMQSettings();
builder.AddConnectionFactoryForRabbit();
builder.AddRabbitConnection();
builder.AddRabbitChannel();

builder.Services.AddSingleton<AddressSubscriber>();
builder.Services.AddSingleton<CustomerSubscriber>();
builder.Services.AddSingleton<OrderSubscriber>();
builder.Services.AddSingleton<ProductSubscriber>();
builder.Services.AddSingleton<VendorSubscriber>();


var app = AppPipeline.Build(builder);

app.Services.GetRequiredService<AddressSubscriber>().ReceiveFromExchange(RabbitMQNames.SyncExchange, RabbitMQEntities.Address);
app.Services.GetRequiredService<CustomerSubscriber>().ReceiveFromExchange(RabbitMQNames.SyncExchange, RabbitMQEntities.Customer);
app.Services.GetRequiredService<OrderSubscriber>().ReceiveFromExchange(RabbitMQNames.SyncExchange, RabbitMQEntities.Order);
app.Services.GetRequiredService<ProductSubscriber>().ReceiveFromExchange(RabbitMQNames.SyncExchange, RabbitMQEntities.Product);
app.Services.GetRequiredService<VendorSubscriber>().ReceiveFromExchange(RabbitMQNames.SyncExchange, RabbitMQEntities.Vendor);




app.Run();

