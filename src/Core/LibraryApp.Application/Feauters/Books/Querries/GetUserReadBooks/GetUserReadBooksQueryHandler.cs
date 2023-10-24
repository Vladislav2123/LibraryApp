using AutoMapper;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Common.Helpers;

namespace LibraryApp.Application.Feauters.Books.Querries.GetUserReadBooks
{
	public class GetUserReadBooksQueryHandler : IRequestHandler<GetUserReadBooksQuery, PagedList<BookLookupDto>>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetUserReadBooksQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedList<BookLookupDto>> Handle(GetUserReadBooksQuery request, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users.Include(user => user.ReadedBooks)
                .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);
            
            if(user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

            IQueryable<Book> booksQuery = user.ReadedBooks.AsQueryable();

            var booksLookupsQuery = _mapper.Map<IQueryable<BookLookupDto>>(user.ReadedBooks);
            return await PagedList<BookLookupDto>.CreateAsync(booksLookupsQuery, request.Page, request.PageSize, cancellationToken);
        }
    }
}
