using FluentValidation;

namespace LibraryApp.Application.Feauters.Users.Commands.Create
{
	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
	{
        public CreateUserCommandValidator()
        {
            RuleFor(command => command.Name)
                .NotEmpty()
                .Length(2, 50);

            RuleFor(command => command.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(command => command.Password)
                .NotEmpty()
                .Length(6, 50);

            RuleFor(command => command.BirthDate)
                .NotEmpty();
        }
    }
}
