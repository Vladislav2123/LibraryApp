using FluentValidation;

namespace LibraryApp.Application.Common.Validators
{
	public class GuidValidator : AbstractValidator<Guid>
	{
		public GuidValidator()
		{
			RuleFor(guid => guid)
				.NotNull()
				.NotEmpty();
		}
	}
}
