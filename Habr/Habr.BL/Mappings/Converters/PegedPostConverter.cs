using AutoMapper;
using Habr.Common.DTOs.Pages;
using Habr.Common.DTOs.V2.Posts;
using Habr.DataAccess.Entities;

namespace Habr.BL.Mappings.Converters;

public class PegedPostConverter : ITypeConverter<PagedList<Post>, PagedResponse<FullPostResponseV2>>

{
    public PagedResponse<FullPostResponseV2> Convert(PagedList<Post> source, PagedResponse<FullPostResponseV2> destination, ResolutionContext context)
    {
        return new PagedResponse<FullPostResponseV2>()
        {
            Data = context.Mapper.Map<IEnumerable<FullPostResponseV2>>(source.Data),
            CurrentPage = source.CurrentPage,
            TotalPages = source.TotalPages
        };
    }
}