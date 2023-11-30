using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CQRS.EndToEndTests.Factories;
using ReadService.Data.Models;
using WriteService.DTOs.Address;
using WriteService.DTOs.Customer;

namespace CQRS.EndToEndTests.Tests;
public class CustomerTests : IClassFixture<ReadServiceWebApplicationFactory<ReadService.Program>>, IClassFixture<WriteServiceWebApplicationFactory<WriteService.Program>>
{
    private readonly HttpClient _readServiceClient;
    private readonly HttpClient _writeServiceClient;

    public CustomerTests(ReadServiceWebApplicationFactory<ReadService.Program> readServiceFactory, WriteServiceWebApplicationFactory<WriteService.Program> writeServiceFactory)
    {
        _readServiceClient = readServiceFactory.CreateClient();
        _writeServiceClient = writeServiceFactory.CreateClient();
    }


    /*
     * COVER:
     * WRITE SERVICE:
     * [POST] /api/customers
     *
     * READ SERVICE:
     * [GET] /api/customers
     */
    [Fact]
    public async Task TestAddAndReadCustomerWithAddress()
    {
        // Arrange
        var customerDto = new CreateCustomerDto("John",
            "Doe",
            "test@test.com",
            "123456789",
            "123456");

        var addressDto = new CreateAddressDto("Test Country",
            "Test Zip Code",
            "Test City",
            "Test Street",
            "Test House Number");

        // Act
        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/customers", customerDto);
        writeResponse.EnsureSuccessStatusCode();
        var customerResponse = await writeResponse.Content.ReadFromJsonAsync<CustomerDto>();

        var writeResponse2 = await _writeServiceClient.PostAsJsonAsync($"/api/customers/{customerResponse.Id}/addresses", addressDto);

        await Task.Delay(TimeSpan.FromSeconds(2));
        
        var readResponse = await _readServiceClient.GetAsync("/api/customers");
        
        var readCustomers = await readResponse.Content.ReadFromJsonAsync<List<Customer>>();

        // Assert
        writeResponse2.EnsureSuccessStatusCode();

        var addressResponse = await writeResponse2.Content.ReadFromJsonAsync<AddressDto>();

        readResponse.EnsureSuccessStatusCode();
        Assert.NotNull(readCustomers);
        Assert.NotNull(addressResponse);

        Assert.True(readCustomers.Any(c =>
            c.FirstName == customerDto.FirstName &&
            c.LastName == customerDto.LastName &&
            c.PhoneNumber == customerDto.PhoneNumber &&
            c.Email == customerDto.Email &&
            c.Addresses.Any() &&

            c.Addresses.ElementAt(0).City == addressResponse.City &&
            c.Addresses.ElementAt(0).ZipCode == addressResponse.ZipCode &&
            c.Addresses.ElementAt(0).Street == addressResponse.Street &&
            c.Addresses.ElementAt(0).HouseNumber == addressResponse.HouseNumber &&
            c.Addresses.ElementAt(0).Id == addressResponse.Id
        ));

        ;

        //Cleanup 
        var deleteResponse = await _writeServiceClient.DeleteAsync($"/api/customers/{customerResponse.Id}");
        deleteResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestUpdateAndReadCustomer()
    {
        // Arrange
        var customerDto = new CreateCustomerDto("John",
            "Doe",
            "test@test.com",
            "123456789",
            "123456");
        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/customers", customerDto);
        writeResponse.EnsureSuccessStatusCode();
        var newCustomerId = (await writeResponse.Content.ReadFromJsonAsync<CustomerDto>())?.Id;
        Assert.NotNull(newCustomerId);

        var updatedCustomer = new UpdateCustomerDto("JohnUpdated", "DoeUpdated");

        //Act
        writeResponse = await _writeServiceClient.PutAsJsonAsync($"/api/customers/{newCustomerId}", updatedCustomer);

        await Task.Delay(TimeSpan.FromSeconds(2));

        var readResponse = await _readServiceClient.GetAsync($"/api/customers/{newCustomerId}");


        var readCustomer = await readResponse.Content.ReadFromJsonAsync<Customer>();

        //Assert
        writeResponse.EnsureSuccessStatusCode();
        readResponse.EnsureSuccessStatusCode();
        Assert.NotNull(readCustomer);
        Assert.Equal(newCustomerId, readCustomer.Id);
        Assert.Equal(updatedCustomer.FirstName, readCustomer.FirstName);
        Assert.Equal(updatedCustomer.LastName, readCustomer.LastName);
        Assert.Equal(customerDto.Email, readCustomer.Email);
        Assert.Equal(customerDto.PhoneNumber, readCustomer.PhoneNumber);

        //Cleanup 
        var deleteResponse = await _writeServiceClient.DeleteAsync($"/api/customers/{newCustomerId}");
        deleteResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task TestUpdateCustomerAddress()
    {
        //Arrange
        var customerDto = new CreateCustomerDto("John",
            "Doe",
            "test@test.com",
            "123456789",
            "123456");


        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/customers", customerDto);
        writeResponse.EnsureSuccessStatusCode();
        var newCustomerId = (await writeResponse.Content.ReadFromJsonAsync<CustomerDto>())?.Id;

        var addresses = new List<AddressDto>();
        for (var i = 0; i < 10; i++)
        {
            var addressDto = new CreateAddressDto($"Test Country{i}",
                $"Test Zip Code{i}",
                $"Test City{i}",
                $"Test Street{i}",
                $"Test House Number{i}");


            var writeResponseAddress = await _writeServiceClient.PostAsJsonAsync($"/api/customers/{newCustomerId}/addresses", addressDto);

            writeResponseAddress.EnsureSuccessStatusCode();

            var addressResponse = await writeResponseAddress.Content.ReadFromJsonAsync<AddressDto>();
            Assert.NotNull(addressResponse);

            addresses.Add(addressResponse);
        }


        var updatedAddress = addresses.Last();
        Assert.NotNull(updatedAddress);
        

        var updateDto = new UpdateAddressDto(updatedAddress.Country + "_UPDATED",
            updatedAddress.ZipCode + "_UPDATED",
            updatedAddress.City + "_UPDATED",
            updatedAddress.Street + "_UPDATED",
            updatedAddress.HouseNumber + "_UPDATED"
        );

        //Act test deleting separate address
        var updateResponse = await _writeServiceClient.PutAsJsonAsync($"/api/customers/{newCustomerId}/addresses/{updatedAddress.Id}", updateDto);
        updateResponse.EnsureSuccessStatusCode();


        await Task.Delay(TimeSpan.FromSeconds(2));

        var readResponse = await _readServiceClient.GetAsync($"/api/customers/{newCustomerId}");

        var readCustomer = await readResponse.Content.ReadFromJsonAsync<Customer>();


        //Assert
        Assert.NotNull(readCustomer);
        Assert.True(readCustomer.Addresses.Any(c => c.ZipCode == "Test Zip Code1"));
        Assert.True(readCustomer.Addresses.Any(c => c.City.Contains("_UPDATED") && c.Id == updatedAddress.Id));

        Assert.Equal(10, readCustomer.Addresses.Count);

    }


    [Fact]
    public async Task TestDeleteCustomerAddress()
    {
        //Arrange
        var customerDto = new CreateCustomerDto("John",
            "Doe",
            "test@test.com",
            "123456789",
            "123456");


        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/customers", customerDto);
        writeResponse.EnsureSuccessStatusCode();
        var newCustomerId = (await writeResponse.Content.ReadFromJsonAsync<CustomerDto>())?.Id;

        var addressIds = new List<long>();
        for (var i = 0; i < 10; i++)
        {
            var addressDto = new CreateAddressDto($"Test Country{i}",
                $"Test Zip Code{i}",
                $"Test City{i}",
                $"Test Street{i}",
                $"Test House Number{i}");


            var writeResponseAddress = await _writeServiceClient.PostAsJsonAsync($"/api/customers/{newCustomerId}/addresses", addressDto);

            writeResponseAddress.EnsureSuccessStatusCode();

            var addressResponse = await writeResponseAddress.Content.ReadFromJsonAsync<AddressDto>();
            Assert.NotNull(addressResponse);

            addressIds.Add(addressResponse.Id);
        }

        //Act test deleting separate address
        var deleteResponse = await _writeServiceClient.DeleteAsync($"/api/customers/{newCustomerId}/addresses/{addressIds.ElementAt(0)}");
        deleteResponse.EnsureSuccessStatusCode();


        await Task.Delay(TimeSpan.FromSeconds(2));

        var readResponse = await _readServiceClient.GetAsync($"/api/customers/{newCustomerId}");

        var readCustomer = await readResponse.Content.ReadFromJsonAsync<Customer>();


        //Assert
        Assert.NotNull(readCustomer);
        Assert.False(readCustomer.Addresses.Any(c => c.ZipCode == "Test Zip Code0"));
        Assert.True(readCustomer.Addresses.Any(c => c.ZipCode == "Test Zip Code1"));
        Assert.Equal(9, readCustomer.Addresses.Count);

    }

    [Fact]
    public async Task TestDeleteCustomer()
    {
        //Arrange
        var customerDto = new CreateCustomerDto("John",
            "Doe",
            "test@test.com",
            "123456789",
            "123456");


        var writeResponse = await _writeServiceClient.PostAsJsonAsync("/api/customers", customerDto);
        writeResponse.EnsureSuccessStatusCode();
        var newCustomerId = (await writeResponse.Content.ReadFromJsonAsync<CustomerDto>())?.Id;

        //Act

        var deleteResponse = await _writeServiceClient.DeleteAsync($"/api/customers/{newCustomerId}");

        await Task.Delay(TimeSpan.FromSeconds(2));

        var readResponse = await _readServiceClient.GetAsync($"/api/customers/{newCustomerId}");

        
        //Assert

        Assert.Equal(HttpStatusCode.NotFound, readResponse.StatusCode);

    }

}
