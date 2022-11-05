using System.Linq.Expressions;
using Habr.Common.DTOs.Pages;
using Habr.DataAccess.Entities;
using Habr.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Habr.DataAccess.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private protected readonly DataContext DataContext;

    protected GenericRepository(DataContext dataContext)
    {
        DataContext = dataContext;
        _dbSet = dataContext.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet.AsSingleQuery();

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<TMapEntity>> GetAllAsync<TMapEntity>(
        Func<IQueryable<TEntity>, IQueryable<TMapEntity>> projectTo,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet.AsSingleQuery();

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        IQueryable<TMapEntity> mapQuery = projectTo(query);

        return await mapQuery.ToListAsync();
    }

    public async Task<PagedList<TEntity>> GetAllAsync(
        PageParameters pageParameters, 
        Expression<Func<TEntity, bool>>? predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool disableTracking = true
        )
    {
        IQueryable<TEntity> query = _dbSet.AsSingleQuery();

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        
        return await query.ToPagedList(
            pageParameters.PageNumber,
            pageParameters.PageSize
        );
    }
    
    public virtual async Task<PagedList<TMapEntity>> GetAllAsync<TMapEntity>(
        Func<IQueryable<TEntity>, IQueryable<TMapEntity>> projectTo,
        PageParameters pageParameters,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet.AsSingleQuery();

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        IQueryable<TMapEntity> mapQuery = projectTo(query);

        return await mapQuery.ToPagedList(
            pageParameters.PageNumber,
            pageParameters.PageSize
        );
    }

    public virtual async Task<TEntity?> GetEntityByIdAsync(
        int id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet.AsSingleQuery();

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(e => e.Id == id);
    }

    public virtual async Task<TMapEntity?> GetEntityByIdAsync<TMapEntity>(
        int id,
        Func<IQueryable<TEntity>, IQueryable<TMapEntity>> projectTo,
        bool disableTracking = true
    )
    {
        IQueryable<TEntity> query = _dbSet.AsSingleQuery();

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        query = query.Where(entity => entity.Id == id);

        IQueryable<TMapEntity> mapQuery = projectTo(query);

        return await mapQuery.SingleOrDefaultAsync();
    }

    public virtual async Task AddEntityAsync(TEntity entity)
    {
        _dbSet.Add(entity);
        await DataContext.SaveChangesAsync();
    }

    public virtual async Task UpdateEntityAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await DataContext.SaveChangesAsync();
    }

    public virtual async Task DeleteEntityAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await DataContext.SaveChangesAsync();
    }
}