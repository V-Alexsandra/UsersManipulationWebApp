using FluentValidation;
using UsersManipulation.Business.DTOs;

namespace UsersManipulation.Business.Validators
{
    public class RegisterUserDtoValidator<T> : AbstractValidator<T> where T : RegisterUserDto
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(r => r.Password)
                .NotEmpty();

            RuleFor(r => r.RepeatPassword)
                .NotEmpty();
              
            RuleFor(r => r.Email)
                .NotNull().WithMessage("Email is required field.");

            When(r => r.Email is not null, () =>
            {
                RuleFor(r => r.Email)
                    .EmailAddress().WithMessage("The entered value is not an email.");
            });
        }
    }
}
