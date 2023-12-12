using FluentValidation;

namespace LibraryApp.Application.Common.Validators
{
	public class PasswordValidator : AbstractValidator<string>
	{
        public PasswordValidator()
        {
            RuleFor(password => password)
                .NotEmpty()
                .Length(6, 50);
        }
    }
}
