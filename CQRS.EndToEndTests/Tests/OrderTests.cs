using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CQRS.EndToEndTests.Factories;
using ReadService.Data.Models;
using WriteService.DTOs.Address;
using WriteService.DTOs.Customer;
using WriteService.DTOs.Order;
using WriteService.DTOs.Product;
using WriteService.DTOs.Vendor;

namespace CQRS.EndToEndTests.Tests;

public class OrderTests : IClassFixture<ReadServiceWebApplicationFactory<ReadService.Program>>,
    IClassFixture<WriteServiceWebApplicationFactory<WriteService.Program>>
{
    private readonly HttpClient _readServiceClient;
    private readonly HttpClient _writeServiceClient;

    public OrderTests(ReadServiceWebApplicationFactory<ReadService.Program> readServiceFactory,
        WriteServiceWebApplicationFactory<WriteService.Program> writeServiceFactory)
    {
        _readServiceClient = readServiceFactory.CreateClient();
        _writeServiceClient = writeServiceFactory.CreateClient();
    }

    [Fact]
    public async Task CreateDuplicityNewOrder()
    {
        //Arrange

        var newCustomer = await CreateCustomer();


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
    public async Task AddToNoExistingCart()
    {
        //Arrange
        var newCustomer = await CreateCustomer();


        //Act



        //Assert

    }



    private async Task<CustomerDto> CreateCustomer()
    {
        var customerDto = new CreateCustomerDto("John",
            "Doe",
            "test@test.com",
            "123456789",
            "123456");

        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/customers", customerDto);
        writeResponse.EnsureSuccessStatusCode();
        var newCustomer = await writeResponse.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(newCustomer);

        return newCustomer;
    }

    private async Task<VendorDto> CreateVendor()
    {
        var vendorDto = new CreateVendorDto("Vendor",
            "Country",
            "Zip Code",
            "City",
            "Street",
            "HouseNumber"
        );

        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/vendors", vendorDto);
        writeResponse.EnsureSuccessStatusCode();
        var newVendor = await writeResponse.Content.ReadFromJsonAsync<VendorDto>();
        Assert.NotNull(newVendor);

        return newVendor;
    }

    private async Task<ProductDto> CreateProduct()
    {
        var vendor = await CreateVendor();

        var productDto = new CreateProductDto("Product Title",
            "Description",
            10,
            15,
            vendor.Id
        );

        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/products", productDto);
        writeResponse.EnsureSuccessStatusCode();
        var newProduct = await writeResponse.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(newProduct);

        return newProduct;
    }
}
