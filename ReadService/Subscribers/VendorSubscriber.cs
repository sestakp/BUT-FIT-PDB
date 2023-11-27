using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using MongoDB.Driver;
using RabbitMQ.Client;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService.Subscribers;

public class VendorSubscriber : RabbitMQReceiver<VendorSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public VendorSubscriber(IModel channel, ILogger<VendorSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
        : base(channel, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as CreateVendorMessage)!;

            var document = new Vendor()
            {
                Id = data.Id,
                Name = data.Name,
                Address = new Address()
                {
                    Country = data.AddressCountry,
                    City = data.AddressCity,
                    ZipCode = data.AddressZipCode,
                    Street = data.AddressStreet,
                    HouseNumber = data.AddressHouseNumber
                }
            };

            database.Collection<Vendor>().InsertOne(document);
        }
    }
}