using AutoMapper;
using Habr.Common.DTOs.V1.Rating;
using Habr.DataAccess.Entities;

namespace Habr.BL.Mappings;

public class RatingProfile : Profile
{
    public RatingProfile()
    {
        CreateMap<AddRatingRequest, Rating>()
            .ForMember(r => r.Value, 
                m => m.MapFrom(r => r.Value));
        
        CreateMap<Rating, RatingResponse>()
            .ForMember(r => r.Id, 
                m => m.MapFrom(r => r.Id))
            .ForMember(r => r.Value, 
                m => m.MapFrom(r => r.Value))
            .ForMember(r => r.PostId, 
                m => m.MapFrom(r => r.PostId))
            .ForMember(r => r.UserId, 
                m => m.MapFrom(r => r.UserId));
    }
}