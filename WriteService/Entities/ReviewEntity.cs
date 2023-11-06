using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities
{
    public class ReviewEntity
    {
        [Key]
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public long ProductId { get; set; }
        public ProductEntity Product { get; set; }
    }
}
