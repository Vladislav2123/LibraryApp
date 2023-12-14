using MediatR;
using LibraryApp.Domain.Enteties;
using LibraryApp.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Models;
using Microsoft.Extensions.Options;
using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LibraryApp.Application.Feauters.Books.Commands.Create;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly FilePaths _filePaths;
	private readonly IHttpContextAccessor _httpContextAccessor;

	private HttpContext HttpContext => _httpContextAccessor.HttpContext;

	public CreateBookCommandHandler(
		ILibraryDbContext dbContext, 
		IOptions<FilePaths> filePathsOptions, 
		IHttpContextAccessor httpContextAccessor)
	{
		_dbContext = dbContext;
		_filePaths = filePathsOptions.Value;
		_httpContextAccessor = httpContextAccessor;
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

		Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.Actor));

		Book newBook = new Book()
		{
			Id = Guid.NewGuid(),
			AuthorId = command.AuthorId,
			Name = command.Name,
			Description = command.Description,
			Year = command.Year,
			ContentPath = contentPath,
			CreationDate = DateTime.Now,
			CreatedUserId = userId
		};

		await _dbContext.Books.AddAsync(newBook, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return newBook.Id;
	}
}
