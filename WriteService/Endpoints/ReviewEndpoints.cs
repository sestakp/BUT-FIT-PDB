using Microsoft.AspNetCore.Mvc;
using WriteService.DTO;
using WriteService.Services;

namespace WriteService.Endpoints
{
    public static class ReviewEndpoints
    {

        public static void MapReviewEndpoints(this WebApplication app)
        {
            string path = "/api/reviews/";

            app.MapPost(path, (ReviewService service, ReviewDto review) =>
            {
                var newReview = service.Create(review);

                if (newReview.Id != default)
                {
                    return Results.Created(path + newReview.Id, newReview);
                }
                return Results.BadRequest();
            });
            
        }
    }
}
