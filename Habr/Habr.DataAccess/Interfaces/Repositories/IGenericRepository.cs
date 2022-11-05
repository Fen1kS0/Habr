using System.Linq.Expressions;
using Habr.Common.DTOs.Pages;
using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Habr.DataAccess.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    );
    
    Task<IEnumerable<TMapEntity>> GetAllAsync<TMapEntity>(
        Func<IQueryable<TEntity>, IQueryable<TMapEntity>> projectTo,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool disableTracking = true
    );
    
    Task<PagedList<TEntity>> GetAllAsync(
        PageParameters pageParameters,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    );
    
    Task<PagedList<TMapEntity>> GetAllAsync<TMapEntity>(
        Func<IQueryable<TEntity>, IQueryable<TMapEntity>> projectTo,
        PageParameters pageParameters,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool disableTracking = true
    );

    Task<TEntity?> GetEntityByIdAsync(
        int id,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool disableTracking = true
    );

    Task<TMapEntity?> GetEntityByIdAsync<TMapEntity>(
        int id,
        Func<IQueryable<TEntity>, IQueryable<TMapEntity>> projectTo,
        bool disableTracking = true
    );

    Task AddEntityAsync(TEntity entity);

    Task UpdateEntityAsync(TEntity entity);

    Task DeleteEntityAsync(TEntity entity);
}