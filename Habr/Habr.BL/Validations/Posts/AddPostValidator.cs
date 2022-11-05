using FluentValidation;
using Habr.Common.DTOs.V1.Posts;

namespace Habr.BL.Validations.Posts;

public class AddPostValidator : AbstractValidator<AddPostRequest>
{
    public AddPostValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("The Title is required")
            .MaximumLength(200).WithMessage("The Title must be less than 200 symbols");
        
        RuleFor(p => p.Text)
            .NotEmpty().WithMessage("The Text is required")
            .MaximumLength(2000).WithMessage("The Text must be less than 2000 symbols");
    }
}