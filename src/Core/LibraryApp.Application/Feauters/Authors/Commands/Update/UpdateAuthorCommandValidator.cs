using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Authors.Commands.Update
{
	public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
	{
        public UpdateAuthorCommandValidator()
        {
			RuleFor(command => command.AuthorId)
				.SetValidator(new GuidValidator());

			RuleFor(command => command.Name)
                .NotEmpty()
                .Length(3, 50);
        }
    }
}
