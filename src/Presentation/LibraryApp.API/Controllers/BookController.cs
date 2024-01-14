using MediatR;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Domain.Models;
using LibraryApp.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Books.Commands.Create;
using LibraryApp.Application.Features.Books.Commands.Delete;
using LibraryApp.Application.Features.Books.Commands.DeleteBookCover;
using LibraryApp.Application.Features.Books.Commands.Update;
using LibraryApp.Application.Features.Books.Commands.UpdateBookCover;
using LibraryApp.Application.Features.Books.Querries.Dto;
using LibraryApp.Application.Features.Books.Querries.GetAllBooks;
using LibraryApp.Application.Features.Books.Querries.GetBook;
using LibraryApp.Application.Features.Books.Querries.GetBookContent;
using LibraryApp.Application.Features.Books.Querries.GetBookCover;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using LibraryApp.Application.Features.Reviews.Queries.GetBookReviews;

namespace LibraryApp.API.Controllers;

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
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult<Guid>> Create(
            [FromForm] CreateBookCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Create), response);
        }

	[HttpPut]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
        public async Task<ActionResult> Update(
            [FromForm] UpdateBookCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

	[HttpDelete("{id}")]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
        public async Task<ActionResult> Delete(
            Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteBookCommand(id);
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

	[HttpGet("{id}/cover")]
	public async Task<ActionResult> GetCover(Guid id, CancellationToken cancellationToken)
	{
            var query = new GetBookCoverQuery(id);
            FileVm response = await _mediator.Send(query, cancellationToken);

            return File(response.Bytes, response.ContentType, response.FileName);
        }

        [HttpPut("{id}/cover")]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult> UpdateCover(
            Guid id, [FromForm] IFormFile coverFile, CancellationToken cancellationToken)
        {
            var command = new UpdateBookCoverCommand(id, coverFile);
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}/cover")]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult> DeleteCover(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteBookCoverCommand(id);
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("{id}/content")]
        public async Task<ActionResult> GetContent(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetBookContentQuery(id);
            FileVm response = await _mediator.Send(query, cancellationToken);

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
