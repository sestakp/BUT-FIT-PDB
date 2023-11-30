using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using Common.RabbitMQ.Messages.Vendor;
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
                Address = new VendorAddress()
                {
                    Country = data.AddressCountry,
                    City = data.AddressCity,
                    ZipCode = data.AddressZipCode,
                    Street = data.AddressStreet,
                    HouseNumber = data.AddressHouseNumber
                }
            };

            database.Collection<Vendor>().InsertOne(document);

            _logger.LogInformation("Inserted new document into Vendors collection.");
        }
    }

    protected override void HandleUpdate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as UpdateVendorMessage)!;

            var filter = Builders<Vendor>
                .Filter
                .Eq(x => x.Id, data.VendorId);

            var updateDefinition = Builders<Vendor>
                .Update
                .Set(x => x.Name, data.Name)
                .Set(x => x.Address.Country, data.Country)
                .Set(x => x.Address.ZipCode, data.ZipCode)
                .Set(x => x.Address.City, data.City)
                .Set(x => x.Address.Street, data.Street)
                .Set(x => x.Address.HouseNumber, data.HouseNumber);

            var result = database.Collection<Vendor>().UpdateOne(filter, updateDefinition);

            _logger.LogInformation("Updated {Count} document in Vendors collection.", result.MatchedCount);
        }
    }
}