using Common.RabbitMQ;
using Common.RabbitMQ.Messages;
using MongoDB.Driver;
using RabbitMQ.Client;
using ReadService.Data;
using ReadService.Data.Models;

namespace ReadService.Subscribers;

public class ReviewSubscriber : RabbitMQReceiver<ReviewSubscriber>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ReviewSubscriber(IModel channel, ILogger<ReviewSubscriber> logger, IServiceScopeFactory serviceScopeFactory)
        : base(channel, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override void HandleCreate(RabbitMQMessage message)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            var data = (message.Data as CreateReviewMessage)!;

            // Insert new document into Reviews colletion
            {
                var review = new Review()
                {
                    Id = data.Id,
                    ProductId = data.ProductId,
                    Rating = data.Rating,
                    Text = data.Text
                };

                database.Collection<Review>().InsertOne(review);

                _logger.LogInformation("Inserted new document into Reviews collection.");
            }
            

            // TODO: calculate real value
            var newTotalRating = Math.Round((new Random()).NextDouble() * 5, 1);

            // Update Rating on collections which contain product identified by data.ProductId
            {
                // Product collection
                {
                    var productFilter = Builders<Product>
                        .Filter
                        .Eq(x => x.Id, data.ProductId);

                    var productUpdateDefinition = Builders<Product>
                        .Update
                        .Set(x => x.Rating, newTotalRating);

                    var result = database
                        .Collection<Product>()
                        .UpdateOne(productFilter, productUpdateDefinition);

                    _logger.LogInformation("Updated {Count} documents in {Collection} collection.",
                        result.MatchedCount,
                        nameof(Product));
                }
                // ProductsOfCategory collection
                {
                    // Get all documents which contain specified product
                    var productsOfCategoryFilter = Builders<ProductsOfCategory>
                        .Filter
                        .ElemMatch(x => x.Products, product => product.Id == data.ProductId);

                    // Update specified products in found documents
                    var productsOfCategoryUpdateDefinition = Builders<ProductsOfCategory>
                        .Update
                        .Set("products.$[].rating", newTotalRating);

                    var result = database
                        .Collection<ProductsOfCategory>()
                        .UpdateOne(productsOfCategoryFilter, productsOfCategoryUpdateDefinition);

                    _logger.LogInformation("Updated {Count} documents in {Collection} collection.",
                        result.MatchedCount,
                        nameof(ProductsOfCategory));
                }
                // ProductsOfSubCategory collection
                {
                    // Get all documents which contain specified product
                    var productsOfSubCategoryFilter = Builders<ProductsOfSubCategory>
                        .Filter
                        .ElemMatch(x => x.Products, product => product.Id == data.ProductId);

                    // Update specified products in found documents
                    var productsOfSubCategoryUpdateDefinition = Builders<ProductsOfSubCategory>
                        .Update
                        .Set("products.$[].rating", newTotalRating);

                    var result = database
                        .Collection<ProductsOfSubCategory>()
                        .UpdateOne(productsOfSubCategoryFilter, productsOfSubCategoryUpdateDefinition);

                    _logger.LogInformation("Updated {Count} documents in {Collection} collection.",
                        result.MatchedCount,
                        nameof(ProductsOfSubCategory));
                }
            }
        }
    }
}