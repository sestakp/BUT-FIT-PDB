using System.Net.Http.Json;
using WriteService.DTOs.Address;
using WriteService.DTOs.Category;
using WriteService.DTOs.Customer;
using WriteService.DTOs.Product;
using WriteService.DTOs.SubCategory;
using WriteService.DTOs.Vendor;
using Xunit;

namespace CQRS.EndToEndTests.Factories;
public class EntityFactory
{
    public EntityFactory()
    {
    }

    public async Task<CustomerDto> CreateCustomer(HttpClient writeServiceClient)
    {
        var customerDto = new CreateCustomerDto("John",
            "Doe",
            "test@test.com",
            "123456789",
            "123456");

        var writeResponse = await writeServiceClient.PostAsJsonAsync("/api/customers", customerDto);
        writeResponse.EnsureSuccessStatusCode();
        var newCustomer = await writeResponse.Content.ReadFromJsonAsync<CustomerDto>();
        Assert.NotNull(newCustomer);

        return newCustomer;
    }

    public async Task<AddressDto> CreateCustomerAddress(HttpClient writeServiceClient, long customerId)
    {
        var addressDto = new CreateAddressDto("Country",
            "Zip Code",
            "City",
            "Street",
            "HouseNumber");

        var writeResponse = await writeServiceClient.PostAsJsonAsync($"/api/customers/{customerId}/addresses", addressDto);
        writeResponse.EnsureSuccessStatusCode();
        var newAddress = await writeResponse.Content.ReadFromJsonAsync<AddressDto>();
        Assert.NotNull(newAddress);

        return newAddress;
    }



    public async Task<VendorDto> CreateVendor(HttpClient writeServiceClient)
    {
        var vendorDto = new CreateVendorDto("Vendor",
            "Country",
            "Zip Code",
            "City",
            "Street",
            "HouseNumber"
        );

        var writeResponse = await writeServiceClient.PostAsJsonAsync("/api/vendors", vendorDto);
        writeResponse.EnsureSuccessStatusCode();
        var newVendor = await writeResponse.Content.ReadFromJsonAsync<VendorDto>();
        Assert.NotNull(newVendor);

        return newVendor;
    }

    public async Task<CategoryDto> CreateCategory(HttpClient writeServiceClient)
    {
        var categoryDto = new CreateCategoryDto("Category",
            "Category Description"
        );

        var writeResponse = await writeServiceClient.PostAsJsonAsync("/api/categories", categoryDto);
        writeResponse.EnsureSuccessStatusCode();
        var newCategory = await writeResponse.Content.ReadFromJsonAsync<CategoryDto>();
        Assert.NotNull(newCategory);

        return newCategory;
    }

    public async Task<SubCategoryDto> CreateSubCategory(HttpClient writeServiceClient, long categoryId)
    {
        var subCategoryDto = new CreateSubCategoryDto("Category",
            "Category Description",
            categoryId
        );

        var writeResponse = await writeServiceClient.PostAsJsonAsync("/api/subCategories", subCategoryDto);
        writeResponse.EnsureSuccessStatusCode();
        var newSubCategory = await writeResponse.Content.ReadFromJsonAsync<SubCategoryDto>();
        Assert.NotNull(newSubCategory);

        return newSubCategory;
    }


    public async Task<ProductDto> CreateProduct(HttpClient writeServiceClient, long vendorId, IEnumerable<string> categories, IEnumerable<string> subcategories)
    {
        var productDto = new CreateProductDto("Product Title",
            "Product description",
            1,
            15,
            vendorId,
            categories,
            subcategories
        );

        var writeResponse = await writeServiceClient.PostAsJsonAsync("/api/products", productDto);
        writeResponse.EnsureSuccessStatusCode();
        var newProduct = await writeResponse.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(newProduct);

        return newProduct;
    }


}