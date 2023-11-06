namespace WriteService.DTO
{
    public class ReviewDto
    {
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; } = null!;
    }
}
