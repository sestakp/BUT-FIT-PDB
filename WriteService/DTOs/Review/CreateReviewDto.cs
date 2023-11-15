namespace WriteService.DTOs.Review;

public record CreateReviewDto(
    int Rating,
    string Text);