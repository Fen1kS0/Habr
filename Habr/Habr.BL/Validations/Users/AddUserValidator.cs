using FluentValidation;
using Habr.Common.DTOs.V1.Users;

namespace Habr.BL.Validations.Users;

public class AddUserValidator : AbstractValidator<UserRegisterRequest>
{
    public AddUserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("The Name is required");
        
        RuleFor(u => u.Password)
            .MinimumLength(6).WithMessage("The Text must be more than 6 symbols");
        
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("The Email is required")
            .EmailAddress().WithMessage("Invalid email")
            .MaximumLength(200).WithMessage("The max length email is 200 symbols");
    }
}