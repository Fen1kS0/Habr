using FluentValidation;
using Habr.Common.DTOs.V1.Rating;

namespace Habr.BL.Validations.Ratings;

public class AddRatingValidator : AbstractValidator<AddRatingRequest>
{
    public AddRatingValidator()
    {
        RuleFor(p => p.Value)
            .Must(v => 1 <= v && v <= 5)
            .WithMessage("Rating must belong range is 1-5 stars");
    }
}