using AutoMapper;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Common.Helpers.Pagination;

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
            User user = await _dbContext.Users.Include(user => user.ReadBooks)
                .FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);
            
            if(user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

            var booksLookups = _mapper.Map<List<BookLookupDto>>(user.ReadBooks);
            return PagedList<BookLookupDto>.Create(booksLookups, request.Page);
        }
    }
}
