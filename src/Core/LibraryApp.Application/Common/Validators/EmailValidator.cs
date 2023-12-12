using FluentValidation;

namespace LibraryApp.Application.Common.Validators
{
	public class EmailValidator : AbstractValidator<string>
	{
        public EmailValidator()
        {
            RuleFor(email => email)
				.NotEmpty()
				.EmailAddress()
				.MaximumLength(100);
		}
    }
}
