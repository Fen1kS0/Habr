using FluentValidation;
using Habr.Common.DTOs.V1.Comments;

namespace Habr.BL.Validations.Comments;

public class AddCommentValidator : AbstractValidator<AddCommentRequest>
{
    public AddCommentValidator()
    {
        RuleFor(c => c.Text)
            .NotEmpty().WithMessage("The Text is required");
    }
}