using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Users.Queries.Login
{
	public class LoginQueriyValidator : AbstractValidator<LoginQuery>
	{
        public LoginQueriyValidator()
        {
            RuleFor(command => command.Email)
                .SetValidator(new EmailValidator());

            RuleFor(command => command.Password)
                .SetValidator(new PasswordValidator());
        }
    }
}
