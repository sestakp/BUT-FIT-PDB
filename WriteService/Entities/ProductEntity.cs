using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities
{
    public class ProductEntity
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PiecesInStock { get; set; }
        public decimal Price { get; set; }
        public bool isDeleted { get; set; }
        
        public long VendorId { get; set; }
        public VendorEntity Vendor { get; set; }
        public IList<ProductCategoryEntity> Categories { get; set; }
        public IList<ProductReviewEntity> Reviews { get; set; }
    }
}
