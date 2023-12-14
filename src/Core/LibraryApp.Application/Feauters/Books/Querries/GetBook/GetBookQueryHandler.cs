using AutoMapper;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBook;

    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
{
	private readonly ILibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBookQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
	{
		Book book = await _dbContext.Books
                .FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

            if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

            return _mapper.Map<BookDto>(book);
        }
}
