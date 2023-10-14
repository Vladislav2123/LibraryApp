using AutoMapper;
using LibraryApp.Application.Books.Querries.Dto;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Books.Querries.GetBooksOfAuthor
{
	public class GetAuthorsBooksQueryHandler : IRequestHandler<GetAuthorsBooksQuery, BooksListVm>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetAuthorsBooksQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BooksListVm> Handle(GetAuthorsBooksQuery query, CancellationToken cancellationToken)
        {
            Author author = await _dbContext.Authors.Include(author => author.Books)
                .FirstOrDefaultAsync(author => author.Id == query.AuthorId, cancellationToken);

            if (author == null) throw new EntityNotFoundException(nameof(Author), query.AuthorId);

            List<BookLookupDto> booksLookups = _mapper.Map<List<BookLookupDto>>(author.Books);
            return new BooksListVm() { Books = booksLookups };      
        }
    }
}
