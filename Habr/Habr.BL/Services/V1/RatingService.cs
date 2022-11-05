using AutoMapper;
using AutoMapper.QueryableExtensions;
using Habr.BL.Interfaces.Services.V1;
using Habr.Common.DTOs.V1.Rating;
using Habr.Common.Exceptions;
using Habr.Common.Resources;
using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;

namespace Habr.BL.Services.V1;

public class RatingService : IRatingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RatingResponse>> GetAllRatings(int postId)
    {
        var ratings = await _unitOfWork.GetRepository<IRatingRepository>().GetAllAsync(
            predicate: c => c.PostId == postId,
            projectTo: q => q.ProjectTo<RatingResponse>(_mapper.ConfigurationProvider)
        );

        return ratings;
    }
    
    public async Task<RatingResponse> GetRatingById(int postId, int id)
    {
        var rating = await _unitOfWork.GetRepository<IRatingRepository>().GetEntityByIdAsync(
            id,
            projectTo: q => q.ProjectTo<RatingResponse>(_mapper.ConfigurationProvider)
        );

        if (rating is null)
        {
            throw new NotFoundException(ExceptionMessageResource.RatingNotFound);
        }
        
        if (rating.PostId != postId)
        {
            throw new NotFoundException(ExceptionMessageResource.RatingNotFound);
        }

        return rating;
    }

    public async Task<RatingResponse> AddRating(int postId, AddRatingRequest addRatingRequest, int userId)
    {
        var rating = await _unitOfWork.GetRepository<IRatingRepository>().GetRatingByPostAndUser(postId, userId);

        if (rating is not null)
        {
            rating.Value = addRatingRequest.Value;
            await _unitOfWork.GetRepository<IRatingRepository>().UpdateEntityAsync(rating);
            return _mapper.Map<RatingResponse>(rating);
        }

        var newRating = _mapper.Map<Rating>(addRatingRequest);
        newRating.PostId = postId;
        newRating.UserId = userId;

        await _unitOfWork.GetRepository<IRatingRepository>().AddEntityAsync(newRating);
        return _mapper.Map<RatingResponse>(newRating);
    }
    
    public async Task RecalculateRatingForPosts()
    {
        var postsId = await _unitOfWork.GetRepository<IPostRepository>().GetPostsId();

        foreach (var postId in postsId)
        {
            var ratings = await _unitOfWork.GetRepository<IRatingRepository>().GetAllAsync(
                predicate: r => r.PostId == postId
            );

            if (!ratings.Any())
            {
                return;
            }

            var averageRating = (decimal) ratings.Average(r => r.Value);

            var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(
                postId,
                disableTracking: false
            );

            post!.Rating = averageRating;
            await _unitOfWork.GetRepository<IPostRepository>().UpdateEntityAsync(post);
        }
    }
}