using System.Transactions;
using Microsoft.EntityFrameworkCore;
using WriteService.Entities;

namespace WriteService.Services
{
    public class VendorService
    {

        private readonly ShopDbContext _context;

        public VendorService(ShopDbContext context)
        {
            _context = context;
        }
        

        public void CreateOrUpdate(VendorEntity vendor)
        {
            using var scope = new TransactionScope();
            // Add your create logic here
            if (vendor.Id != default)
            {
                _context.Vendors.Update(vendor);
            }
            else
            {
                _context.Vendors.Add(vendor);
            }
            _context.SaveChanges();

            scope.Complete();
        }

        public void SoftDeleteVendor(int vendorId)
        {
            using var scope = new TransactionScope();
            var vendor = _context.Vendors
                .Include(v => v.Products) // Include the related products
                .FirstOrDefault(v => v.Id == vendorId);

            if (vendor != null)
            {
                vendor.isDeleted = true;

                foreach (var product in vendor.Products)
                {
                    product.isDeleted = true;
                }

                _context.SaveChanges();
            }

            scope.Complete();
        }
    }
}
