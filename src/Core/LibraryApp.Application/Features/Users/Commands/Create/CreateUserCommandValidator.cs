using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Features.Users.Commands.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
	public CreateUserCommandValidator()
	{
		RuleFor(command => command.Name)
			.NotEmpty()
			.Length(2, 50);

		RuleFor(command => command.Email)
			.SetValidator(new EmailValidator());

		RuleFor(command => command.Password)
			.SetValidator(new PasswordValidator());

		RuleFor(command => command.BirthDate)
			.NotEmpty();
	}
}
