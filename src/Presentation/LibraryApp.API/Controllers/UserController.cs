using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Feauters.Books.Querries.GetUserReadBooks;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Feauters.Reviews.Queries.GetUserReviews;
using LibraryApp.Application.Feauters.Users.Commands.Create;
using LibraryApp.Application.Feauters.Users.Commands.Delete;
using LibraryApp.Application.Feauters.Users.Commands.Update;
using LibraryApp.Application.Feauters.Users.Queries.Dto;
using LibraryApp.Application.Feauters.Users.Queries.GetUserDetails;
using LibraryApp.Application.Feauters.Users.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [ApiController]
	[Route("api/users")]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediator;

		public UserController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<UserLookupDto>>> GetAll(
			string? search, string? sortColumn, string? sortOrder, int page, int size)
		{
			var query = new GetAllUsersQuery(search, sortColumn, sortOrder, new Page(page, size));
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserDetailsDto>> GetById(Guid id)
		{
			var query = new GetUserQuery(id);
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpGet("{id}/books")]
		public async Task<ActionResult<PagedList<BookLookupDto>>> GetReadBooks(Guid id, int page, int size)
		{
			var query = new GetUserReadBooksQuery(id, new Page(page, size));
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpGet("{id}/reviews")]
		public async Task<ActionResult<PagedList<ReviewDto>>> GetReviews(Guid id, int page, int size)
		{
			var query = new GetUserReviewsQuery(id, new Page(page, size));
			var resopnse = await _mediator.Send(query);

			return Ok(resopnse);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] CreateUserCommand command)
		{
			var response = await _mediator.Send(command);

			return CreatedAtAction(nameof(Create), response);
		}

		[HttpPut]
		public async Task<ActionResult> Update([FromBody] UpdateUserCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			var command = new DeleteUserCommand(id);
			await _mediator.Send(command);

			return NoContent();
		}
	}
}
