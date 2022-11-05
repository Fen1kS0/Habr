using AutoMapper;
using Habr.Common.DTOs.V1.Comments;
using Habr.DataAccess.Entities;

namespace Habr.BL.Mappings;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, FullCommentResponse>()
            .ForMember(c => c.Id, 
                m => m.MapFrom(c => c.Id))
            .ForMember(c => c.Text, 
                m => m.MapFrom(c => c.Text))
            .ForMember(c => c.AuthorName, 
                m => m.MapFrom(c => c.Author.Name))
            .ForMember(c => c.ChildComments, 
                m => m.MapFrom(c => c.Comments))
            .ForMember(c => c.CreatedAt, 
                m => m.MapFrom(c => c.CreatedAt));

        CreateMap<Comment, CommentResponse>()
            .ForMember(c => c.Id, 
                m => m.MapFrom(c => c.Id))
            .ForMember(c => c.Text, 
                m => m.MapFrom(c => c.Text))
            .ForMember(c => c.AuthorName, 
                m => m.MapFrom(c => c.Author.Name))
            .ForMember(c => c.CreatedAt, 
                m => m.MapFrom(c => c.CreatedAt));

        CreateMap<AddCommentRequest, Comment>()
            .ForMember(c => c.Text,
                m => m.MapFrom(c => c.Text))
            .ForMember(c => c.ParentCommentId,
                m => m.MapFrom(c => c.ParentCommentId));
        
        CreateMap<UpdateCommentRequest, Comment>()
            .ForMember(c => c.Text,
                m => m.MapFrom(c => c.Text));
        
        CreateMap<Comment, CommentAddedResponse>()
            .ForMember(c => c.Id, 
                m => m.MapFrom(c => c.Id))
            .ForMember(c => c.Text,
                m => m.MapFrom(c => c.Text))
            .ForMember(c => c.AuthorId,
                m => m.MapFrom(c => c.AuthorId))
            .ForMember(c => c.PostId,
                m => m.MapFrom(c => c.PostId))
            .ForMember(c => c.ParentCommentId,
                m => m.MapFrom(c => c.ParentCommentId));
    }
}