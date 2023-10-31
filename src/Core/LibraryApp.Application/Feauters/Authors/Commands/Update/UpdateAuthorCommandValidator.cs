using FluentValidation;

namespace LibraryApp.Application.Feauters.Authors.Commands.Update
{
	public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
	{
        public UpdateAuthorCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.AuthorId)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.Name)
                .NotEmpty()
                .Length(3, 50);
        }
    }
}
