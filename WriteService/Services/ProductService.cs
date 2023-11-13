using WriteService.DTOs.Product;
using WriteService.DTOs.Review;
using WriteService.Entities;
using WriteService.Exceptions;

namespace WriteService.Services;

public class ProductService
{
    private readonly ShopDbContext _context;

    public ProductService(ShopDbContext context)
    {
        _context = context;
    }

    public ProductEntity Create(CreateProductDto dto)
    {
        var vendor = _context
            .Vendors
            .Find(dto.VendorId);

        if (vendor is null)
        {
            throw new Exception("Specified vendor does not exist.");
        }

        var product = new ProductEntity()
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            PiecesInStock = dto.PricesInStock,
            VendorId = dto.VendorId
        };

        _context.Add(product);
        _context.SaveChanges();

        return product;
    }

    public ReviewEntity AddReview(long productId, CreateReviewDto dto)
    {
        var product = FindProduct(productId);

        var review = new ReviewEntity()
        {
            Rating = dto.Rating,
            Text = dto.Text,
            Product = product
        };

        _context.Add(review);
        _context.SaveChanges();

        return review;
    }

    public void Delete(long productId)
    {
        var product = FindProduct(productId);

        product.IsDeleted = true;

        _context.Update(product);
        _context.SaveChanges();
    }

    private ProductEntity FindProduct(long id)
    {
        var product = _context
            .Products
            .FirstOrDefault(v => v.Id == id);

        if (product is null)
        {
            throw new EntityNotFoundException(id);
        }

        return product;
    }
}