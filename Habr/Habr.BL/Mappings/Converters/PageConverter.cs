using AutoMapper;
using Habr.Common.DTOs.Pages;
using Habr.DataAccess.Entities;

namespace Habr.BL.Mappings.Converters;

public class PageConverter<T> : ITypeConverter<PagedList<T>, PagedResponse<T>>
{
    public PagedResponse<T> Convert(PagedList<T> source, PagedResponse<T> destination, ResolutionContext context)
    {
        return new PagedResponse<T>()
        {
            Data = source.Data,
            CurrentPage = source.CurrentPage,
            TotalPages = source.TotalPages
        };
    }
}