using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities
{
    public class ProductReviewEntity
    {
        [Key]
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
    }
}
