namespace Habr.DataAccess.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;
    
    public UnitOfWork(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public TRepository GetRepository<TRepository>()
    {
        return (TRepository) _serviceProvider.GetService(typeof(TRepository))!;
    }
}