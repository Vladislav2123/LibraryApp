using FluentValidation;
using LibraryApp.Application.Common.Validators;

namespace LibraryApp.Application.Feauters.Authors.Commands.Create
{
	public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
	{
		public CreateAuthorCommandValidator() 
		{
			RuleFor(command => command.UserId)
				.SetValidator(new GuidValidator());

			RuleFor(command => command.Name)
				.NotEmpty()
				.Length(3, 50);
		}
	}
}
