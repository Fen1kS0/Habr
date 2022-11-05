using AutoMapper;
using AutoMapper.QueryableExtensions;
using Habr.BL.Interfaces.Services.V3;
using Habr.Common.DTOs.Pages;
using Habr.Common.DTOs.V2.Posts;
using Habr.Common.Exceptions;
using Habr.Common.Resources;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;

namespace Habr.BL.Services.V3;

public class PostServiceV3 : IPostServiceV3
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PostServiceV3(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResponse<PostResponseV2>> GetAllPostsAsync(PageParameters pageParameters)
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            projectTo: q => q.ProjectTo<PostResponseV2>(_mapper.ConfigurationProvider),
            pageParameters: pageParameters
        );

        return _mapper.Map<PagedResponse<PostResponseV2>>(posts);
    }

    public async Task<PagedResponse<FullPostResponseV2>> GetAllPublicPostsAsync(PageParameters pageParameters)
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            include: IncludeEntities.IncludeAllForPublicPost,
            pageParameters: pageParameters
        );

        return _mapper.Map<PagedResponse<FullPostResponseV2>>(posts);
    }

    public async Task<PagedResponse<FullPostResponseV2>> GetPublicPostsByUserAsync(int authorId, PageParameters pageParameters)
    {
        await UserExistsAsync(authorId);

        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.Author.Id == authorId && p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            include: IncludeEntities.IncludeAllForPublicPost,
            pageParameters: pageParameters
        );

        return _mapper.Map<PagedResponse<FullPostResponseV2>>(posts);
    }

    public async Task<PagedResponse<PostResponseV2>> GetAllDraftPostsAsync(PageParameters pageParameters)
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => !p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.UpdatedAt),
            projectTo: q => q.ProjectTo<PostResponseV2>(_mapper.ConfigurationProvider),
            pageParameters: pageParameters
        );

        return _mapper.Map<PagedResponse<PostResponseV2>>(posts);
    }

    public async Task<PagedResponse<PostResponseV2>> GetDraftPostsByUserAsync(int authorId, PageParameters pageParameters)
    {
        await UserExistsAsync(authorId);

        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.Author.Id == authorId && !p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.UpdatedAt),
            projectTo: q => q.ProjectTo<PostResponseV2>(_mapper.ConfigurationProvider),
            pageParameters: pageParameters
        );

        return _mapper.Map<PagedResponse<PostResponseV2>>(posts);
    }

    /// <summary>
    /// Checking for the existence of a user
    /// </summary>
    /// <exception cref="NotFoundException">If user not found</exception>
    private async Task UserExistsAsync(int userId)
    {
        var user = await _unitOfWork.GetRepository<IUserRepository>().GetEntityByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException(ExceptionMessageResource.UserNotFound);
        }
    }
}