using AutoMapper;
using Habr.Common.DTOs.V1.Comments;
using Habr.Common.DTOs.V2.Posts;
using Habr.DataAccess.Entities;

namespace Habr.BL.Mappings.Resolvers;

public class TreeCommentsResolverV2 : IValueResolver<Post, FullPostResponseV2, IEnumerable<FullCommentResponse>>
{
    public IEnumerable<FullCommentResponse> Resolve(Post source, FullPostResponseV2 destination, IEnumerable<FullCommentResponse> destMember, ResolutionContext context)
    {
        foreach (var comment in source.Comments)
        {
            if (comment.ParentCommentId is not null)
            {
                var parentComment = source.Comments.First(c => c.Id == comment.ParentCommentId);
                parentComment.Comments.Add(comment);
            }
        }

        return context.Mapper.Map<IEnumerable<FullCommentResponse>>(source.Comments.Where(c => c.ParentCommentId == null));
    }
}