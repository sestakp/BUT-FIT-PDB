namespace WriteService.DTO.Review;

public record ReviewDto(
    long Id,
    int Rating,
    string Text);