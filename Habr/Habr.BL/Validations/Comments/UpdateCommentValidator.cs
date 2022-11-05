using FluentValidation;
using Habr.Common.DTOs.V1.Comments;

namespace Habr.BL.Validations.Comments;

public class UpdateCommentValidator : AbstractValidator<UpdateCommentRequest>
{
    public UpdateCommentValidator()
    {
        RuleFor(c => c.Text)
            .NotEmpty().WithMessage("The Text is required");
    }
}