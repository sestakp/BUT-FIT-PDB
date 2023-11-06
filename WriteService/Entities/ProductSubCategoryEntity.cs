using System.ComponentModel.DataAnnotations;

namespace WriteService.Entities
{
    public class ProductSubCategoryEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
