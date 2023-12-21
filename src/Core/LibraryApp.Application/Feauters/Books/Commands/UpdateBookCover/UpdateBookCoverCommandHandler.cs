﻿using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Books.Commands.UpdateBookCover;

public class UpdateBookCoverCommandHandler : IRequestHandler<UpdateBookCoverCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly FilePaths _filePaths;

	public UpdateBookCoverCommandHandler(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper,
		IOptions<FilePaths> filePathsOptions)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
		_filePaths = filePathsOptions.Value;
	}

	public async Task<Unit> Handle(UpdateBookCoverCommand command, CancellationToken cancellationToken)
	{
		var book = await _dbContext.Books
			.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

		if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

		if (string.IsNullOrEmpty(book.CoverPath))
		{
			string newCoverFileName = $"{Guid.NewGuid()}{Path.GetExtension(command.CoverFile.FileName)}";
			book.CoverPath = Path.Combine(_filePaths.CoversPath, newCoverFileName);
		}
		else _fileWrapper.DeleteFile(book.CoverPath);

		await _fileWrapper.SaveFileAsync(command.CoverFile, book.CoverPath, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
