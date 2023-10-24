using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Books.Commands.Delete
{
	public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

        public DeleteBookCommandHandler(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
		{
			Book book = await _dbContext.Books.FirstOrDefaultAsync(book => book.Id == command.Id, cancellationToken);

			if(book == null) throw new EntityNotFoundException(nameof(Book), command.Id);

			_dbContext.Books.Remove(book);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
