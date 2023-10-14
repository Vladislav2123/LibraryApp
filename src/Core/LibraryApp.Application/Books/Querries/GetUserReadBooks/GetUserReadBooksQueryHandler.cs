using AutoMapper;
using LibraryApp.Application.Books.Querries.Dto;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Books.Querries.GetUserReadBooks
{
	public class GetUserReadBooksQueryHandler : IRequestHandler<GetUserReadBooksQuery, BooksListVm>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetUserReadBooksQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BooksListVm> Handle(GetUserReadBooksQuery query, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users.Include(user => user.ReadedBooks)
                .FirstOrDefaultAsync(user => user.Id == query.UserId, cancellationToken);
            
            if(user == null) throw new EntityNotFoundException(nameof(User), query.UserId);

            List<BookLookupDto> booksLookups = _mapper.Map<List<BookLookupDto>>(user.ReadedBooks);
            return new BooksListVm() { Books = booksLookups };
        }
    }
}
