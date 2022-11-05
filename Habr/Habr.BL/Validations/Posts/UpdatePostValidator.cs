using FluentValidation;
using Habr.Common.DTOs.V1.Posts;

namespace Habr.BL.Validations.Posts;

public class UpdatePostValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostValidator()
    {
        RuleFor(p => p.Text).NotNull();
        
        RuleFor(p => p.Title)
            .NotEmpty().WithMessage("The Title is required");
    }
}