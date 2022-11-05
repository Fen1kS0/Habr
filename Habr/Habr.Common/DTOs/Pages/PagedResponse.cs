namespace Habr.Common.DTOs.Pages;

public class PagedResponse<T> 
{
    public IEnumerable<T> Data { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}