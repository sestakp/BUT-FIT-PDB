namespace WriteService.DTO.Review;

public record CreateReviewDto(
    int Rating,
    string Text);