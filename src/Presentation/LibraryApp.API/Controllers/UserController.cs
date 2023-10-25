using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Feauters.Books.Querries.GetUserReadBooks;
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
		public async Task<ActionResult<PagedList<UserLookupDto>>> Get(
			string? search, string? sortColumn, string? sortOrder, int page, int limit)
		{
			var query = new GetUsersQuery(search, sortColumn, sortOrder, page, limit);
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<UserDetailsDto>> GetById(Guid id)
		{
			var query = new GetUserDetailsQuery(id);
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpGet("{id}/books")]
		public async Task<ActionResult<PagedList<BookLookupDto>>> GetReadedBooks(Guid id, int page, int pageSize)
		{
			var query = new GetUserReadBooksQuery(id, page, pageSize);
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] CreateUserCommand command)
		{
			var response = await _mediator.Send(command);

			return CreatedAtAction(nameof(GetById), response);
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
			var response = await _mediator.Send(command);

			return NoContent();
		}
	}
}
