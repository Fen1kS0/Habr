namespace Habr.DataAccess.Entities;

public class PagedList<T>
{
    public PagedList(ICollection<T> data, int currentPage, int totalPages, int pageSize, int totalCount)
    {
        Data = data;
        CurrentPage = currentPage;
        TotalPages = totalPages;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public ICollection<T> Data { get; private set; }
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}