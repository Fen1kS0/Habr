using AutoMapper;
using AutoMapper.QueryableExtensions;
using Habr.BL.Interfaces.Services.V2;
using Habr.Common.DTOs.V2.Posts;
using Habr.Common.Exceptions;
using Habr.Common.Resources;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;
using Microsoft.EntityFrameworkCore;

namespace Habr.BL.Services.V2;

public class PostServiceV2 : IPostServiceV2
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PostServiceV2(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostResponseV2>> GetAllPostsAsync()
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            projectTo: q => q.ProjectTo<PostResponseV2>(_mapper.ConfigurationProvider)
        );

        return posts;
    }

    public async Task<IEnumerable<FullPostResponseV2>> GetAllPublicPostsAsync()
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            include: IncludeEntities.IncludeAllForPublicPost
        );

        return _mapper.Map<IEnumerable<FullPostResponseV2>>(posts);
    }

    public async Task<IEnumerable<FullPostResponseV2>> GetPublicPostsByUserAsync(int authorId)
    {
        await UserExistsAsync(authorId);

        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.Author.Id == authorId && p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            include: IncludeEntities.IncludeAllForPublicPost
        );

        return _mapper.Map<IEnumerable<FullPostResponseV2>>(posts);
    }

    public async Task<IEnumerable<PostResponseV2>> GetAllDraftPostsAsync()
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => !p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.UpdatedAt),
            projectTo: q => q.ProjectTo<PostResponseV2>(_mapper.ConfigurationProvider)
        );

        return posts;
    }

    public async Task<IEnumerable<PostResponseV2>> GetDraftPostsByUserAsync(int authorId)
    {
        await UserExistsAsync(authorId);

        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.Author.Id == authorId && !p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.UpdatedAt),
            projectTo: q => q.ProjectTo<PostResponseV2>(_mapper.ConfigurationProvider)
        );

        return posts;
    }

    public async Task<FullPostResponseV2> GetPostByIdAsync(int id)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(
            id,
            include: q => q
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
        );

        if (post is null)
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);

        return _mapper.Map<FullPostResponseV2>(post);
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