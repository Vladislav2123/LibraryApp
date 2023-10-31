using FluentValidation;

namespace LibraryApp.Application.Feauters.Users.Commands.Update
{
	public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
        public UpdateUserCommandValidator()
        {
            RuleFor(command => command.Id)
                .NotNull()
                .NotEmpty();

			RuleFor(command => command.Name)
				.NotEmpty()
				.Length(2, 50);

			RuleFor(command => command.Email)
				.NotEmpty()
				.EmailAddress()
				.MaximumLength(100);

			RuleFor(command => command.BirthDate)
				.NotNull()
				.NotEmpty();
		}
    }
}
