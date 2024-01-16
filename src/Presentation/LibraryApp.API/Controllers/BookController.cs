using LibraryApp.Application.Features.Books.Commands.DeleteBookCover;
using LibraryApp.Application.Features.Reviews.Queries.GetBookReviews;
using LibraryApp.Application.Features.Books.Commands.UpdateBookCover;
using LibraryApp.Application.Features.Books.Querries.GetBookContent;
using LibraryApp.Application.Features.Books.Querries.GetBookCover;
using LibraryApp.Application.Features.Books.Querries.GetAllBooks;
using LibraryApp.Application.Features.Books.Querries.GetBook;
using LibraryApp.Application.Features.Books.Commands.Create;
using LibraryApp.Application.Features.Books.Commands.Update;
using LibraryApp.Application.Features.Books.Commands.Delete;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using LibraryApp.Application.Features.Books.Querries.Dto;
using Microsoft.AspNetCore.Authorization;
using LibraryApp.Application.Pagination;
using LibraryApp.API.Authorization;
using LibraryApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LibraryApp.API.Controllers;
[Route("api/books")]
public class BookController : ControllerBase
{
	public BookController(IMediator mediator, IAuthorizationService authorizationService)
		: base(mediator, authorizationService) { }

	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<PagedList<BookLookupDto>>> GetAll(
		string? search, Guid? author, string? sortColumn, string? sortOrder,
		int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetAllBooksQuery(search, author, sortColumn, sortOrder, new Page(page, size));
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		[FromForm] CreateBookCommand command, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var response = await Mediator.Send(command, cancellationToken);

		return CreatedAtAction(nameof(Create), response);
	}

	[HttpPut]
	public async Task<ActionResult> Update(
			[FromForm] UpdateBookCommand command, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(
			Guid id, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var command = new DeleteBookCommand(id);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpGet("{id}")]
	[AllowAnonymous]
	public async Task<ActionResult<BookDto>> GetById(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetBookQuery(id);
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpGet("{id}/cover")]
	[AllowAnonymous]
	public async Task<ActionResult> GetCover(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetBookCoverQuery(id);
		FileVm response = await Mediator.Send(query, cancellationToken);

		return File(response.Bytes, response.ContentType, response.FileName);
	}

	[HttpPut("{id}/cover")]
	public async Task<ActionResult> UpdateCover(
		Guid id, [FromForm] IFormFile coverFile, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var command = new UpdateBookCoverCommand(id, coverFile);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{id}/cover")]
	public async Task<ActionResult> DeleteCover(Guid id, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var command = new DeleteBookCoverCommand(id);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpGet("{id}/content")]
	[AllowAnonymous]
	public async Task<ActionResult> GetContent(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetBookContentQuery(id);
		FileVm response = await Mediator.Send(query, cancellationToken);

		return File(response.Bytes, response.ContentType, response.FileName);
	}

	[HttpGet("{id}/reviews")]
	[AllowAnonymous]
	public async Task<ActionResult<PagedList<ReviewDto>>> GetReviews(
			Guid id, string? sortColumn, string? sortOrder, int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetBookReviewsQuery(id, sortColumn, sortOrder, new Page(page, size));
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

}
