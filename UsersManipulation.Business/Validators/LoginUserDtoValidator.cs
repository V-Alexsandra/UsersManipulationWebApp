using FluentValidation;
using UsersManipulation.Business.DTOs.UserDtos;

namespace UsersManipulation.Business.Validators
{
    public class LoginUserDtoValidator<T> : AbstractValidator<T> where T : LoginUserDto
    {
        public LoginUserDtoValidator()
        {
            RuleFor(l => l.Email)
                .NotNull().WithMessage("Email is required field.");

            When(l => l.Email is not null, () =>
            {
                RuleFor(l => l.Email)
                    .EmailAddress().WithMessage("The entered value is not an email.");
            });

            RuleFor(l => l.Password)
                .NotNull().WithMessage("Password is required field.");
        }
    }
}
