using FluentValidation;

namespace LibraryApp.Application.Features.Authors.Commands.Create;

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
	public CreateAuthorCommandValidator() 
	{
		RuleFor(command => command.Name)
			.NotEmpty()
			.Length(3, 50);
	}
}
