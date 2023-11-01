using LibraryApp.Application.Interfaces;
using MediatR;
using LibraryApp.Domain.Enteties;
using LibraryApp.Application.Common.Exceptions;

namespace LibraryApp.Application.Feauters.Books.Commands.Create
{
	public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
	{
		private readonly ILibraryDbContext _dbContext;

		public CreateBookCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
		{
			if (_dbContext.Authors.Any(author => 
				author.Id == command.AuthorId) == false)
			{
				throw new EntityNotFoundException(nameof(Author), command.AuthorId);
			}

			if (_dbContext.Books.Any(book =>
				book.AuthorId == command.AuthorId &&
				book.Name == command.Name &&
				book.Description == command.Description &&
				book.Year == command.Year))
			{
				throw new EntityAlreadyExistException(nameof(Book));
			}

			Book newBook = new Book()
			{
				Id = Guid.NewGuid(),
				AuthorId = command.AuthorId,
				Name = command.Name,
				Description = command.Description,
				Year = command.Year,
				Text = command.Text,
				CreationDate = DateTime.Now,
				CreatedUserId = command.UserId
			};

			await _dbContext.Books.AddAsync(newBook, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return newBook.Id;
		}
	}
}
