﻿using LibraryApp.Application.Interfaces;
using MediatR;
using LibraryApp.Domain.Enteties;
using LibraryApp.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Models;
using Microsoft.Extensions.Options;

namespace LibraryApp.Application.Feauters.Books.Commands.Create
{
	public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly FilePaths _filePaths;

		public CreateBookCommandHandler(ILibraryDbContext dbContext, IOptions<FilePaths> filePathsOptions)
		{
			_dbContext = dbContext;
			_filePaths = filePathsOptions.Value;
		}

		public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
		{
			if (await _dbContext.Authors
				.AnyAsync(author => author.Id == command.AuthorId, cancellationToken) == false)
			{
				throw new EntityNotFoundException(nameof(Author), command.AuthorId);
			}

			if (await _dbContext.Books
				.AnyAsync(book =>
					book.AuthorId == command.AuthorId &&
					book.Name == command.Name &&
					book.Description == command.Description &&
					book.Year == command.Year, cancellationToken))
			{
				throw new EntityAlreadyExistException(nameof(Book));
			}

			string newContentFileName = $"{Guid.NewGuid()}{Path.GetExtension(command.ContentFile.FileName)}";
			string contentPath = Path.Combine(_filePaths.BooksPath, newContentFileName);

			using (FileStream stream = new FileStream(contentPath, FileMode.Create))
			{
				await command.ContentFile.CopyToAsync(stream, cancellationToken);
			}

			Book newBook = new Book()
			{
				Id = Guid.NewGuid(),
				AuthorId = command.AuthorId,
				Name = command.Name,
				Description = command.Description,
				Year = command.Year,
				ContentPath = contentPath,
				CreationDate = DateTime.Now,
				CreatedUserId = command.UserId
			};

			await _dbContext.Books.AddAsync(newBook, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return newBook.Id;
		}
	}
}
