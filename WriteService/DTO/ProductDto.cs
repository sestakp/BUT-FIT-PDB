namespace WriteService.DTO
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PiecesInStock { get; set; }
        public decimal Price { get; set; }
        public bool isDeleted { get; set; }
    }
}
