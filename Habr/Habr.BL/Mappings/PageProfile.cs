using AutoMapper;
using Habr.BL.Mappings.Converters;
using Habr.Common.DTOs.Pages;
using Habr.Common.DTOs.V2.Posts;
using Habr.DataAccess.Entities;

namespace Habr.BL.Mappings;

public class PageProfile : Profile
{
    public PageProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedResponse<>))
            .ConvertUsing(typeof(PageConverter<>));
        
        CreateMap<PagedList<Post>, PagedResponse<FullPostResponseV2>>()
            .ConvertUsing<PegedPostConverter>();
    }
}