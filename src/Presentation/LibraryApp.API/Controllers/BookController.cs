using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Application.Feauters.Books.Querries.GetBooks;
using LibraryApp.Application.Feauters.Books.Querries.GetBook;
using LibraryApp.Application.Feauters.Books.Commands.Create;
using LibraryApp.Application.Feauters.Books.Commands.Update;
using LibraryApp.Application.Feauters.Books.Commands.Delete;

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
        public async Task<ActionResult<PagedList<BookLookupDto>>> Get(
            string? search, Guid? author, string? sortColumn, string? sortOrder, int page, int limit)
        {
            var query = new GetBooksQuery(search, author, sortColumn, sortOrder, page, limit);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(Guid id)
        {
            var query = new GetBookQuery(id);
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateBookCommand command)
        {
            var response = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), response);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateBookCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteBookCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }

    }
}
