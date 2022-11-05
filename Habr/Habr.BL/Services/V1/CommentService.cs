using AutoMapper;
using AutoMapper.QueryableExtensions;
using Habr.BL.Interfaces.Services.V1;
using Habr.Common.DTOs.V1.Comments;
using Habr.Common.Exceptions;
using Habr.Common.Resources;
using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Habr.DataAccess.UoW;

namespace Habr.BL.Services.V1;

public class CommentService : ICommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentsAsync(int postId)
    {
        var comments = await _unitOfWork.GetRepository<ICommentRepository>()
            .GetAllAsync(
                predicate: c => c.PostId == postId,
                projectTo: q => q.ProjectTo<CommentResponse>(_mapper.ConfigurationProvider)
            );

        return comments;
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentsByUserAsync(int postId, int authorId)
    {
        var comments = await _unitOfWork.GetRepository<ICommentRepository>()
            .GetAllAsync(
                predicate: c => c.AuthorId == authorId && c.PostId == postId,
                projectTo: q => q.ProjectTo<CommentResponse>(_mapper.ConfigurationProvider)
            );

        return comments;
    }

    public async Task<CommentResponse> GetCommentsByIdAsync(int postId, int id)
    {
        var comment = await _unitOfWork.GetRepository<ICommentRepository>()
            .GetEntityByIdAsync(
                id,
                projectTo: q => q.ProjectTo<CommentResponse>(_mapper.ConfigurationProvider)
            );
        if (comment is null)
        {
            throw new NotFoundException(ExceptionMessageResource.CommentNotFound);
        }

        if (comment.PostId != postId)
        {
            throw new BusinessException(ExceptionMessageResource.PostNotOwnComment);
        }

        return comment;
    }

    public async Task<CommentAddedResponse> AddCommentAsync(int postId, AddCommentRequest addCommentRequest,
        int authorId)
    {
        await UserExistsAsync(authorId);
        await PostExistsAsync(postId);

        if (addCommentRequest.ParentCommentId is not null)
        {
            await ParentCommentExistsAsync(addCommentRequest.ParentCommentId.Value, postId);
        }

        var comment = _mapper.Map<Comment>(addCommentRequest);
        comment.PostId = postId;
        comment.AuthorId = authorId;

        await _unitOfWork.GetRepository<ICommentRepository>().AddEntityAsync(comment);

        return _mapper.Map<CommentAddedResponse>(comment);
    }

    public async Task UpdateCommentAsync(int postId, int id, UpdateCommentRequest updateCommentRequest, int authorId)
    {
        var comment = await _unitOfWork.GetRepository<ICommentRepository>()
            .GetEntityByIdAsync(id, disableTracking: false);
        if (comment is null)
        {
            throw new NotFoundException(ExceptionMessageResource.CommentNotFound);
        }

        if (comment.PostId != postId)
        {
            throw new BusinessException(ExceptionMessageResource.PostNotOwnComment);
        }

        await UserExistsAsync(authorId);

        if (comment.AuthorId != authorId)
        {
            throw new AccessDeniedException(ExceptionMessageResource.UserNotOwnComment);
        }

        _mapper.Map(updateCommentRequest, comment);

        await _unitOfWork.GetRepository<ICommentRepository>().UpdateEntityAsync(comment);
    }
    
    public async Task UpdateCommentAsync(int postId, int id, UpdateCommentRequest updateCommentRequest)
    {
        var comment = await _unitOfWork.GetRepository<ICommentRepository>()
            .GetEntityByIdAsync(id, disableTracking: false);
        if (comment is null)
        {
            throw new NotFoundException(ExceptionMessageResource.CommentNotFound);
        }

        if (comment.PostId != postId)
        {
            throw new BusinessException(ExceptionMessageResource.PostNotOwnComment);
        }

        _mapper.Map(updateCommentRequest, comment);

        await _unitOfWork.GetRepository<ICommentRepository>().UpdateEntityAsync(comment);
    }

    public async Task DeleteCommentAsync(int postId, int id, int authorId)
    {
        var comment = await _unitOfWork.GetRepository<ICommentRepository>()
            .GetEntityByIdAsync(id, disableTracking: false);
        if (comment == null)
        {
            throw new NotFoundException(ExceptionMessageResource.CommentNotFound);
        }

        if (comment.PostId != postId)
        {
            throw new BusinessException(ExceptionMessageResource.PostNotOwnComment);
        }

        await UserExistsAsync(authorId);

        if (comment.AuthorId != authorId)
        {
            throw new AccessDeniedException(ExceptionMessageResource.UserNotOwnComment);
        }

        await _unitOfWork.GetRepository<ICommentRepository>().DeleteEntityAsync(comment);
    }
    
    public async Task DeleteCommentAsync(int postId, int id)
    {
        var comment = await _unitOfWork.GetRepository<ICommentRepository>()
            .GetEntityByIdAsync(id, disableTracking: false);
        if (comment == null)
        {
            throw new NotFoundException(ExceptionMessageResource.CommentNotFound);
        }

        if (comment.PostId != postId)
        {
            throw new BusinessException(ExceptionMessageResource.PostNotOwnComment);
        }

        await _unitOfWork.GetRepository<ICommentRepository>().DeleteEntityAsync(comment);
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

    /// <summary>
    /// Checking for the existence of a post
    /// </summary>
    /// <exception cref="NotFoundException">If post not found</exception>
    /// <exception cref="BusinessException">If post don't published</exception>
    private async Task PostExistsAsync(int postId)
    {
        var post = await _unitOfWork.GetRepository<IPostRepository>().GetEntityByIdAsync(postId);
        if (post is null)
        {
            throw new NotFoundException(ExceptionMessageResource.PostNotFound);
        }

        if (!post.IsPublished)
            throw new BusinessException(ExceptionMessageResource.PostNotPublished);
    }

    /// <summary>
    /// Checking for the existence of a comment in this post
    /// </summary>
    /// <exception cref="NotFoundException">If parent comment not found</exception>
    /// <exception cref="BusinessException">If parent comment not belong this post</exception>
    private async Task ParentCommentExistsAsync(int commentId, int postId)
    {
        var comment = await _unitOfWork.GetRepository<ICommentRepository>().GetEntityByIdAsync(commentId);
        if (comment is null)
        {
            throw new NotFoundException(ExceptionMessageResource.CommentNotFound);
        }

        if (comment.PostId != postId)
        {
            throw new BusinessException(ExceptionMessageResource.PostNotOwnComment);
        }
    }
}