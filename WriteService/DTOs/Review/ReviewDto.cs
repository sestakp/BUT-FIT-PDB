namespace WriteService.DTOs.Review;

public record ReviewDto(
    long Id,
    int Rating,
    string Text);