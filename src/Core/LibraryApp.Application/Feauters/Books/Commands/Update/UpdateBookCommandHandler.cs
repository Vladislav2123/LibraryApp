using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Books.Commands.Update;
public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;

	public UpdateBookCommandHandler(ILibraryDbContext dbContext, IFileWrapper fileWrapper)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
	}

	public async Task<Unit> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
	{
		var book = await _dbContext.Books
			.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

		if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

		if (await _dbContext.Authors
			.AnyAsync(author => author.Id == command.AuthorId, cancellationToken) == false)
			throw new EntityNotFoundException(nameof(Author), command.AuthorId);

		var sameBook = await _dbContext.Books
			.FirstOrDefaultAsync(book =>
				book.AuthorId == command.AuthorId &&
				book.Name == command.Name &&
				book.Description == command.Description &&
				book.Year == command.Year, cancellationToken);

		
		if (sameBook != null)
		{
			// If there is other book with the same properties, EntityAlreadyExist trowing
			if (sameBook.Id != book.Id) throw new EntityAlreadyExistException(nameof(Book));

			// If this book has the same properties and content file don`t updating, EntityHasNoChanges trowing
			if (command.ContentFile == null) throw new EntityHasNoChangesException(nameof(Book), command.BookId);
		}

		book.AuthorId = command.AuthorId;
		book.Name = command.Name;
		book.Description = command.Description;
		book.Year = command.Year;

		//if (command.ContentFile != null &&
		//	IsContentsEqual(book.ContentPath, command.ContentFile) == false)
		if (command.ContentFile != null)
		{
			_fileWrapper.DeleteFile(book.ContentPath);
			await _fileWrapper.SaveFileAsync(command.ContentFile, book.ContentPath, cancellationToken);
		}

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}

	//private bool IsContentsEqual(string currentContentPath, IFormFile commandContent)
	//{
	//	PdfReader currentPdfReader = new PdfReader(currentContentPath);

	//	using (var stream = commandContent.OpenReadStream())
	//	{
	//		PdfReader commandPdfReader = new PdfReader(stream);

	//		if (currentPdfReader.NumberOfPages != commandPdfReader.NumberOfPages)
	//		{
	//			currentPdfReader.Close();
	//			commandPdfReader.Close();
	//			return false;
	//		}

	//		for (int i = 1; i <= currentPdfReader.NumberOfPages; i++)
	//		{
	//			byte[] page1Bytes = currentPdfReader.GetPageContent(i);
	//			byte[] page2Bytes = commandPdfReader.GetPageContent(i);

	//			if (page1Bytes.Length == page2Bytes.Length)
	//			{
	//				currentPdfReader.Close();
	//				commandPdfReader.Close();
	//				return true;
	//			}
	//		}

	//		currentPdfReader.Close();
	//		commandPdfReader.Close();
	//	}

	//	return false;
	//}
}
