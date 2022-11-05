using Habr.Common.DTOs.V1.Rating;

namespace Habr.BL.Interfaces.Services.V1;

public interface IRatingService
{
    Task<IEnumerable<RatingResponse>> GetAllRatings(int postId);
    Task<RatingResponse> GetRatingById(int postId, int id);
    Task<RatingResponse> AddRating(int postId, AddRatingRequest addRatingRequest, int userId);
    Task RecalculateRatingForPosts();
}