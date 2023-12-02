using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CQRS.EndToEndTests.Factories;
using ReadService.Data.Models;
using WriteService.DTOs.Address;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.DTOs.Order;
using WriteService.DTOs.Product;
using WriteService.DTOs.SubCategory;
using WriteService.DTOs.Vendor;

namespace CQRS.EndToEndTests.Tests;

public class OrderTests : IClassFixture<ReadServiceWebApplicationFactory<ReadService.Program>>,
    IClassFixture<WriteServiceWebApplicationFactory<WriteService.Program>>, 
    IClassFixture<EntityFactory>
{
    private readonly HttpClient _readServiceClient;
    private readonly HttpClient _writeServiceClient;
    private readonly EntityFactory _entityFactory;

    public OrderTests(ReadServiceWebApplicationFactory<ReadService.Program> readServiceFactory,
        WriteServiceWebApplicationFactory<WriteService.Program> writeServiceFactory, EntityFactory entityFactory)
    {
        _entityFactory = entityFactory;
        _readServiceClient = readServiceFactory.CreateClient();
        _writeServiceClient = writeServiceFactory.CreateClient();
    }

    [Fact]
    public async Task CreateDuplicityNewOrder()
    {
        //Arrange

        var newCustomer = await _entityFactory.CreateCustomer(_writeServiceClient);


        var createOrderDto = new CreateOrderDto(newCustomer.Id);

        //Act

        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/orders", createOrderDto);


        var writeResponse2 = await _writeServiceClient.PostAsJsonAsync("/api/orders", createOrderDto);


        await Task.Delay(TimeSpan.FromSeconds(2));

        var readResponse = await _readServiceClient.GetAsync($"/api/orders/?customerId={newCustomer.Id}");

        //Assert

        writeResponse.EnsureSuccessStatusCode();
        var orderId = await writeResponse.Content.ReadFromJsonAsync<long>();


        writeResponse2.EnsureSuccessStatusCode();

        var orderId2 = await writeResponse2.Content.ReadFromJsonAsync<long>();

        Assert.Equal(orderId, orderId2);

        readResponse.EnsureSuccessStatusCode();
        var orders = await readResponse.Content.ReadFromJsonAsync<List<Order>>();
        Assert.NotNull(orders);
        Assert.Empty(orders);

    }


    [Fact]
    public async Task CompleteOrder()
    {
        //Arrange
        var newCustomer = await _entityFactory.CreateCustomer(_writeServiceClient);
        var newVendor = await _entityFactory.CreateVendor(_writeServiceClient);

        var categories = new List<CategoryDto>();
        var subCategories = new List<SubCategoryDto>();

        for (int i = 0; i < 5; i++)
        {
            var category = await _entityFactory.CreateCategory(_writeServiceClient);

            for (int j = 0; j < 3; j++)
            {
                var subCategory = await _entityFactory.CreateSubCategory(_writeServiceClient, category.Id);

                if ((i + j) % 3 == 0)
                {

                    subCategories.Add(subCategory);
                }
            }

            if (i % 2 == 0)
            {
                categories.Add(category);
            }
        }



        var product = await _entityFactory.CreateProduct(_writeServiceClient, newVendor.Id, categories.Select(c => c.NormalizedName), subCategories.Select(c => c.NormalizedName));
        
        var createOrderDto = new CreateOrderDto(newCustomer.Id);
        //Act


        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/orders", createOrderDto);

        writeResponse.EnsureSuccessStatusCode();

        var orderId = await writeResponse.Content.ReadFromJsonAsync<long>();

        writeResponse = await _writeServiceClient.PostAsync($"/api/orders/{orderId}/add-to-cart/{product.Id}", null);

        writeResponse.EnsureSuccessStatusCode();

        var completeOrderDto = new CompleteOrderDto("Country1", "ZipCode1", "City1", "Street1", "HouseNumber1");
        writeResponse = await _writeServiceClient.PutAsJsonAsync($"/api/orders/{orderId}/complete", completeOrderDto);

        writeResponse.EnsureSuccessStatusCode();


        await Task.Delay(TimeSpan.FromSeconds(2)); 

        var readResponse = await _readServiceClient.GetAsync($"/api/products/{product.Id}");

        readResponse.EnsureSuccessStatusCode();

        var readedProduct = await readResponse.Content.ReadFromJsonAsync<Product>();

        var readResponse2 = await _readServiceClient.GetAsync($"/api/orders?customerId={newCustomer.Id}");
        readResponse2.EnsureSuccessStatusCode();
        var readerOrders = await readResponse2.Content.ReadFromJsonAsync<List<Order>>();

        //Assert
        Assert.NotNull(readerOrders);
        Assert.NotNull(readedProduct);
        Assert.Equal(1, readerOrders.Count);
        Assert.Equal(product.PricesInStock - 1, readedProduct.PiecesInStock);
        Assert.Equal(1, readerOrders.First().Products.Count());
    }


    [Fact]
    public async Task TestCompleteOrderWithOutOfStockProduct()
    {
        //Arrange
        var customers = new List<CustomerDto>()
        {
            await _entityFactory.CreateCustomer(_writeServiceClient),
            await _entityFactory.CreateCustomer(_writeServiceClient)
        };
        var newVendor = await _entityFactory.CreateVendor(_writeServiceClient);

        var category = await _entityFactory.CreateCategory(_writeServiceClient);
        var product = await _entityFactory.CreateProduct(_writeServiceClient, newVendor.Id, new List<string>{ category.NormalizedName }, new List<string>());



        var createOrderDto = new CreateOrderDto(customers.First().Id);

        var writeResponse1 = await _writeServiceClient.PostAsJsonAsync("/api/orders", createOrderDto);
        writeResponse1.EnsureSuccessStatusCode();
        var orderId1 = await writeResponse1.Content.ReadFromJsonAsync<long>();
        
        writeResponse1 = await _writeServiceClient.PostAsync($"/api/orders/{orderId1}/add-to-cart/{product.Id}", null);
        writeResponse1.EnsureSuccessStatusCode();



        var createOrderDto2 = new CreateOrderDto(customers.Last().Id);

        var writeResponse2 = await _writeServiceClient.PostAsJsonAsync("/api/orders", createOrderDto2);
        writeResponse2.EnsureSuccessStatusCode();
        var orderId2 = await writeResponse2.Content.ReadFromJsonAsync<long>();

        writeResponse2 = await _writeServiceClient.PostAsync($"/api/orders/{orderId2}/add-to-cart/{product.Id}", null);
        writeResponse2.EnsureSuccessStatusCode();


        //Let second customer add to cart, then complete order
        var completeOrderDto = new CompleteOrderDto("Country1", "ZipCode1", "City1", "Street1", "HouseNumber1");
        writeResponse1 = await _writeServiceClient.PutAsJsonAsync($"/api/orders/{orderId1}/complete", completeOrderDto);
        writeResponse1.EnsureSuccessStatusCode();

        //Act

        completeOrderDto = new CompleteOrderDto("Country1", "ZipCode1", "City1", "Street1", "HouseNumber1");
        writeResponse2 = await _writeServiceClient.PutAsJsonAsync($"/api/orders/{orderId2}/complete", completeOrderDto);

        //Assert
        Assert.Equal(HttpStatusCode.InternalServerError, writeResponse2.StatusCode);
        

    }




}
