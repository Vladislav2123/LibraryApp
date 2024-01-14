using FluentValidation;
using LibraryApp.Application.Common.Validators;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Features.Users.Commands.UpdateUserRole;
public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(command => command.UserId)
            .SetValidator(new GuidValidator());

        RuleFor(command => command.Role)
            .NotEmpty()
            .Must(role => Enum.TryParse<UserRole>(role, out var result) == true)
            .WithMessage("Invalid role");
    }
}
