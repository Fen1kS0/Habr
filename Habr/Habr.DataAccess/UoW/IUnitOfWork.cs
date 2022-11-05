namespace Habr.DataAccess.UoW;

public interface IUnitOfWork
{
    TRepository GetRepository<TRepository>();
}