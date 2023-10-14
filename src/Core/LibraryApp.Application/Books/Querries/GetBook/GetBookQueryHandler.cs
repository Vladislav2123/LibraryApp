using AutoMapper;
using LibraryApp.Application.Books.Querries.Dto;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Books.Querries.GetBook
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
	{
		private readonly ILibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBookQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(GetBookQuery query, CancellationToken cancellationToken)
		{
			Book book = await _dbContext.Books.FirstOrDefaultAsync(book => book.Id == query.Id, cancellationToken);

            if (book == null) throw new EntityNotFoundException(nameof(Book), query.Id);

            return _mapper.Map<BookDto>(book);
        }
	}
}
