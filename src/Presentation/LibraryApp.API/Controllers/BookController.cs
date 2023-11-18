using LibraryApp.Application.Feauters.Books.Querries.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Application.Feauters.Books.Querries.GetBooks;
using LibraryApp.Application.Feauters.Books.Querries.GetBook;
using LibraryApp.Application.Feauters.Books.Commands.Create;
using LibraryApp.Application.Feauters.Books.Commands.Update;
using LibraryApp.Application.Feauters.Books.Commands.Delete;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Feauters.Reviews.Queries.GetBookReviews;
using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Feauters.Books.Querries.GetBookContent;

namespace LibraryApp.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<BookLookupDto>>> GetAll(
            string? search, Guid? author, string? sortColumn, string? sortOrder, 
            int page, int size, CancellationToken cancellationToken)
        {
            var query = new GetAllBooksQuery(search, author, sortColumn, sortOrder, new Page(page, size));
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromForm] CreateBookCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Create), response);
        }

        [HttpPut]
        public async Task<ActionResult> Update(
            [FromForm] UpdateBookCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete()]
        public async Task<ActionResult> Delete(
            DeleteBookCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookQuery(id);
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(response);
        }

        [HttpGet("{id}/content")]
        public async Task<ActionResult<BookContentVm>> GetContent(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookContentQuery(id);
            BookContentVm response = await _mediator.Send(query, cancellationToken);

            return File(response.Bytes, response.ContentType, response.FileName);
        }

        [HttpGet("{id}/reviews")]
        public async Task<ActionResult<PagedList<ReviewDto>>> GetReviews(
            Guid id, string? sortColumn, string? sortOrder, int page, int size, CancellationToken cancellationToken)
        {
            var query = new GetBookReviewsQuery(id, sortColumn, sortOrder, new Page(page, size));
            var response = await _mediator.Send(query, cancellationToken);

            return Ok(response);
        }

    }
}
