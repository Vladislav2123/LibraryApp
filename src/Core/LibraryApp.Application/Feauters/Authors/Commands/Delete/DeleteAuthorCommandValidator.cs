using FluentValidation;

namespace LibraryApp.Application.Feauters.Authors.Commands.Delete
{
	public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
	{
        public DeleteAuthorCommandValidator()
        {
            RuleFor(command => command.UserId)
                .NotNull()
                .NotEmpty();

            RuleFor(command => command.AuthorId)
                .NotNull()
                .NotEmpty();
        }
    }
}
