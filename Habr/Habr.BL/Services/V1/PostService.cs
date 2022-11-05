using AutoMapper;
using AutoMapper.QueryableExtensions;
using Habr.BL.Interfaces.Services.V1;
using Habr.Common.DTOs.V1.Posts;
using Habr.Common.Exceptions;
using Habr.Common.Resources;
using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Habr.BL.Services.V1;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PostService> _logger;

    public PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PostResponse>> GetAllPostsAsync()
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            projectTo: q => q.ProjectTo<PostResponse>(_mapper.ConfigurationProvider)
        );

        return posts;
    }

    public async Task<IEnumerable<FullPostResponse>> GetAllPublicPostsAsync()
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            include: IncludeEntities.IncludeAllForPublicPost
        );

        return _mapper.Map<IEnumerable<FullPostResponse>>(posts);
    }

    public async Task<IEnumerable<FullPostResponse>> GetPublicPostsByUserAsync(int authorId)
    {
        await UserExistsAsync(authorId);

        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.Author.Id == authorId && p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.CreatedAt),
            include: IncludeEntities.IncludeAllForPublicPost
        );

        return _mapper.Map<IEnumerable<FullPostResponse>>(posts);
    }

    public async Task<IEnumerable<PostResponse>> GetAllDraftPostsAsync()
    {
        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => !p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.UpdatedAt),
            projectTo: q => q.ProjectTo<PostResponse>(_mapper.ConfigurationProvider)
        );

        return posts;
    }

    public async Task<IEnumerable<PostResponse>> GetDraftPostsByUserAsync(int authorId)
    {
        await UserExistsAsync(authorId);

        var posts = await _unitOfWork.GetRepository<IPostRepository>().GetAllAsync(
            predicate: p => p.Author.Id == authorId && !p.IsPublished,
            orderBy: q => q.OrderByDescending(p => p.UpdatedAt),
            projectTo: q => q.ProjectTo<PostResponse>(_mapper.ConfigurationProvider)
        );

        return posts;
    }

    public async Task<FullPostResponse> GetPostByIdAsync(int id)
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

        return _mapper.Map<FullPostResponse>(post);
    }

    public async Task<PostAddedResponse> AddPostAsync(AddPostRequest addPostRequest, int authorId)
    {
        await UserExistsAsync(authorId);

        var post = _mapper.Map<Post>(addPostRequest);
        post.AuthorId = authorId;

        await _unitOfWork.GetRepository<IPostRepository>().AddEntityAsync(post);

        if (post.IsPublished)
        {
            _logger.LogInformation(InformationMessageResource.PostPublished);
        }

        return _mapper.Map<PostAddedResponse>(post);
    }

    public async Task UpdatePostAsync(int id, UpdatePostRequest updatePostRequest, int authorId)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(id, disableTracking: false);
        if (post is null)
        {
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);
        }

        await UserExistsAsync(authorId);

        if (post.AuthorId != authorId)
        {
            throw new AccessDeniedException(ExceptionMessageResource.UserNotOwnPost);
        }

        if (post.IsPublished)
        {
            throw new BusinessException(ExceptionMessageResource.UpdatePublishPost);
        }

        _mapper.Map(updatePostRequest, post);

        await _unitOfWork.GetRepository<IPostRepository>().UpdateEntityAsync(post);
    }
    
    
    public async Task UpdatePostAsync(int id, UpdatePostRequest updatePostRequest)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(id, disableTracking: false);
        if (post is null)
        {
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);
        }

        _mapper.Map(updatePostRequest, post);

        await _unitOfWork.GetRepository<IPostRepository>().UpdateEntityAsync(post);
    }

    public async Task DeletePostAsync(int id, int authorId)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(id, disableTracking: false);
        if (post is null)
        {
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);
        }

        await UserExistsAsync(authorId);

        if (post.AuthorId != authorId)
        {
            throw new AccessDeniedException(ExceptionMessageResource.UserNotOwnPost);
        }

        await _unitOfWork.GetRepository<IPostRepository>().DeleteEntityAsync(post);
    }
    
    public async Task DeletePostAsync(int id)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(id, disableTracking: false);
        if (post is null)
        {
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);
        }

        await _unitOfWork.GetRepository<IPostRepository>().DeleteEntityAsync(post);
    }

    public async Task PublishPostAsync(int id, int authorId)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(id, disableTracking: false);
        if (post is null)
        {
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);
        }

        await UserExistsAsync(authorId);

        if (post.AuthorId != authorId)
        {
            throw new AccessDeniedException(ExceptionMessageResource.UserNotOwnPost);
        }

        post.IsPublished = true;

        await _unitOfWork.GetRepository<IPostRepository>().UpdateEntityAsync(post);

        _logger.LogInformation(InformationMessageResource.PostPublished);
    }

    public async Task MoveToDraftAsync(int id, int authorId)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(
            id,
            include: q => q.Include(pp => pp.Comments),
            disableTracking: false
        );
        if (post is null)
        {
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);
        }

        if (post.Comments.Count != 0)
        {
            throw new BusinessException(ExceptionMessageResource.PostHasComments);
        }

        await UserExistsAsync(authorId);

        if (post.AuthorId != authorId)
        {
            throw new AccessDeniedException(ExceptionMessageResource.UserNotOwnPost);
        }

        post.IsPublished = false;

        await _unitOfWork.GetRepository<IPostRepository>().UpdateEntityAsync(post);
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