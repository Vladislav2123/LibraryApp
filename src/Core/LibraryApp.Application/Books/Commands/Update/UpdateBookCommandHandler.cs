﻿using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Books.Commands.Update
{
	public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public UpdateBookCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = _dbContext;
		}

		public async Task<Unit> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
		{
			Book book = await _dbContext.Books.FirstOrDefaultAsync(book => book.Id == command.Id, cancellationToken);

			if(book == null) throw new EntityNotFoundException(nameof(Book), command.Id);

			book.AuthorId = command.AuthorId;
			book.Name = command.Name;
			book.Description = command.Description;
			book.Year = command.Year;
			book.Text = command.Text;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
