using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WriteService.DTO;
using WriteService.Entities;

namespace WriteService.Services
{
    public class ProductService
    {
        private readonly ShopDbContext _context;
        private readonly IMapper _mapper;
        public ProductService(ShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public ProductEntity Create(ProductDto productDto)
        {
            using var scope = new TransactionScope();
            
            var productEntity = _mapper.Map<ProductEntity>(productDto);

            var vendor = _context.Vendors.Find(productEntity.VendorId);

            if (vendor == null)
            {
                scope.Dispose();
                return new ProductEntity();
            }

            _context.Products.Add(productEntity);

            _context.SaveChanges();

            scope.Complete();
            return productEntity;
        }
        public bool SoftDelete(long productId)
        {
            using var scope = new TransactionScope();
            var product = _context.Products
                //.Include(v => v.Reviews) // Include the related products
                .FirstOrDefault(v => v.Id == productId);

            if (product != null)
            {
                product.isDeleted = true;
                /*
                foreach (var review in product.Reviews)
                {
                    review.isDeleted = true;
                }*/

                _context.SaveChanges();
            }

            scope.Complete();

            return product != null;
        }
    }
}
