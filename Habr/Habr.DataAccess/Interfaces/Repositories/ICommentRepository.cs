using Habr.DataAccess.Entities;

namespace Habr.DataAccess.Interfaces.Repositories;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task LoadChildCommentsAsync(Comment comment);
    Task LoadAuthorAsync(Comment comment);
    Task LoadChildTreeCommentsAsync(Comment comment);
}