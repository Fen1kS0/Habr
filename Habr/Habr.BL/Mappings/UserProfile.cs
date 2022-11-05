using AutoMapper;
using Habr.Common;
using Habr.Common.DTOs.V1.Users;
using Habr.DataAccess.Entities;

namespace Habr.BL.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRegisterRequest, User>()
            .ForMember(u => u.Email, m => 
                m.MapFrom(u => u.Email))
            .ForMember(u => u.Name, m => 
                m.MapFrom(u => u.Name))
            .ForMember(u => u.Password, m => 
                m.MapFrom(u => Hash.HashPassword(u.Password)))
            .ForMember(u => u.Role, m => 
                m.MapFrom(u => u.Role));

        CreateMap<User, UserResponse>()
            .ForMember(u => u.Email, m =>
                m.MapFrom(u => u.Email))
            .ForMember(u => u.Name, m =>
                m.MapFrom(u => u.Name));
    }
}