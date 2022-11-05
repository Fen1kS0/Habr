using AutoMapper;
using Habr.BL.Mappings.Resolvers;
using Habr.Common.DTOs.V1.Posts;
using Habr.Common.DTOs.V2.Posts;
using Habr.DataAccess.Entities;
namespace Habr.BL.Mappings;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<AddPostRequest, Post>()
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title))
            .ForMember(p => p.IsPublished,
                m => m.MapFrom(p => p.IsPublished))
            .ReverseMap();

        CreateMap<Post, PostAddedResponse>()
            .ForMember(p => p.Id,
                m => m.MapFrom(p => p.Id))
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title))
            .ForMember(p => p.AuthorId,
                m => m.MapFrom(p => p.AuthorId));

        CreateMap<UpdatePostRequest, Post>()
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title));

        CreateMap<Post, FullPostResponse>()
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title))
            .ForMember(p => p.AuthorEmail,
                m => m.MapFrom(p => p.Author.Email))
            .ForMember(p => p.PublishDate,
                m => m.MapFrom(p => p.CreatedAt))
            .ForMember(p => p.LastUpdatedDate,
                m => m.MapFrom(p => p.UpdatedAt))
            .ForMember(p => p.Rating,
                m => m.MapFrom(p => p.Rating))
            .ForMember(p => p.TreeComments,
                m => m.MapFrom<TreeCommentsResolver>());

        CreateMap<Post, PostAddedResponse>()
            .ForMember(p => p.Id,
                m => m.MapFrom(p => p.Id))
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title))
            .ForMember(p => p.AuthorId,
                m => m.MapFrom(p => p.AuthorId));
        
        CreateMap<Post, PostResponse>()
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title))
            .ForMember(p => p.AuthorEmail,
                m => m.MapFrom(p => p.Author.Email))
            .ForMember(p => p.PublishDate,
                m => m.MapFrom(p => p.CreatedAt))
            .ForMember(p => p.LastUpdatedDate,
                m => m.MapFrom(p => p.UpdatedAt))
            .ForMember(p => p.Rating,
                m => m.MapFrom(p => p.Rating));
        
        CreateMap<Post, FullPostResponseV2>()
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title))
            .ForMember(p => p.Author,
                m => m.MapFrom(p => p.Author))
            .ForMember(p => p.PublishDate,
                m => m.MapFrom(p => p.CreatedAt))
            .ForMember(p => p.LastUpdatedDate,
                m => m.MapFrom(p => p.UpdatedAt))
            .ForMember(p => p.Rating,
                m => m.MapFrom(p => p.Rating))
            .ForMember(p => p.TreeComments,
                m => m.MapFrom<TreeCommentsResolverV2>());
        
        CreateMap<Post, PostResponseV2>()
            .ForMember(p => p.Text,
                m => m.MapFrom(p => p.Text))
            .ForMember(p => p.Title,
                m => m.MapFrom(p => p.Title))
            .ForMember(p => p.Author,
                m => m.MapFrom(p => p.Author))
            .ForMember(p => p.PublishDate,
                m => m.MapFrom(p => p.CreatedAt))
            .ForMember(p => p.LastUpdatedDate,
                m => m.MapFrom(p => p.UpdatedAt))
            .ForMember(p => p.Rating,
                m => m.MapFrom(p => p.Rating));
    }
}