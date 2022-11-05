using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;

namespace Habr.DataAccess.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(DataContext dataContext) : base(dataContext)
    {
    }

    public override async Task DeleteEntityAsync(Comment comment)
    {
        await LoadChildCommentsAsync(comment);

        while (comment.Comments.Any())
        {
            await DeleteEntityAsync(comment.Comments.First());
        }

        await base.DeleteEntityAsync(comment);
    }

    public async Task LoadChildCommentsAsync(Comment comment)
    {
        await DataContext.Entry(comment).Collection(c => c.Comments).LoadAsync();
    }

    public async Task LoadAuthorAsync(Comment comment)
    {
        await DataContext.Entry(comment).Reference(c => c.Author).LoadAsync();
    }
    
    public async Task LoadChildTreeCommentsAsync(Comment comment)
    {
        await LoadChildCommentsAsync(comment);
        await LoadAuthorAsync(comment);

        foreach (var childComment in comment.Comments)
        {
            await LoadChildTreeCommentsAsync(childComment);
        }
    }
}