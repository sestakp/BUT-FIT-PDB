﻿namespace WriteService.Entities;

public class ReviewEntity : EntityBase
{
    public required int Rating { get; init; }
    public required string Text { get; init; }

    // Foreign keys
    public long ProductId { get; init; }
    public long CustomerId { get; init; }
}