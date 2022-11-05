using Habr.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habr.DataAccess;

public static class QueryableExtensions
{
    public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var totalPages = (int) Math.Ceiling(count / (double) pageSize);
        pageNumber = pageNumber > totalPages ? totalPages : pageNumber;
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return new PagedList<T>(items, pageNumber, totalPages, pageSize, count);
    }
}