﻿using FluentValidation;

namespace LibraryApp.Application.Feauters.Books.Commands.Update
{
	public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
	{
        public UpdateBookCommandValidator()
        {
			RuleFor(command => command.UserId)
				.NotNull()
				.NotEmpty();

			RuleFor(command => command.BookId)
				.NotNull()
				.NotEmpty();

			RuleFor(command => command.AuthorId)
				.NotNull()
				.NotEmpty();

			RuleFor(command => command.Name)
				.NotEmpty()
				.MaximumLength(50);

			RuleFor(command => command.Description)
				.MaximumLength(1000);

			RuleFor(command => command.Year)
				.Must(year => year == 0 || (year > 1000 && year < 2025))
				.WithMessage("Year must be in the range from 1000 to 2025 or 0");
		}
    }
}