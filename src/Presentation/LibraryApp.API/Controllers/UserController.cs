using LibraryApp.API.Authorization;
using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Feauters.Books.Querries.GetUserReadBooks;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Feauters.Reviews.Queries.GetUserReviews;
using LibraryApp.Application.Feauters.Users.Commands.AddReadedBook;
using LibraryApp.Application.Feauters.Users.Commands.Create;
using LibraryApp.Application.Feauters.Users.Commands.Delete;
using LibraryApp.Application.Feauters.Users.Commands.DeleteReadBook;
using LibraryApp.Application.Feauters.Users.Commands.DeleteUserAvatar;
using LibraryApp.Application.Feauters.Users.Commands.Update;
using LibraryApp.Application.Feauters.Users.Commands.UpdateUserAvatar;
using LibraryApp.Application.Feauters.Users.Queries.Dto;
using LibraryApp.Application.Feauters.Users.Queries.GetUserAvatar;
using LibraryApp.Application.Feauters.Users.Queries.GetUserDetails;
using LibraryApp.Application.Feauters.Users.Queries.GetUsers;
using LibraryApp.Application.Feauters.Users.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
	[ApiController]
	[Route("api/users")]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IAuthorizationService _authorizationService;

		public UserController(IMediator mediator, IAuthorizationService authorizationService)
		{
			_mediator = mediator;
			_authorizationService = authorizationService;
		}

		[HttpGet("login")]
		public async Task<ActionResult<LoginResponse>> Login(
			LoginQuery command, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(command, cancellationToken);

			return Ok(response);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Register(
			[FromBody] CreateUserCommand command, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(command, cancellationToken);

			return CreatedAtAction(nameof(Register), response);
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<UserLookupDto>>> GetAll(
			string? search, string? sortColumn, string? sortOrder,
			int page, int size, CancellationToken cancellationToken)
		{
			var query = new GetAllUsersQuery(search, sortColumn, sortOrder, new Page(page, size));
			var response = await _mediator.Send(query, cancellationToken);

			return Ok(response);
		}

		[HttpPut]
		[Authorize]
		public async Task<ActionResult> Update(
			[FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
		{
			var authorizationResult = await _authorizationService
				.AuthorizeAsync(User, command.UserId, Policies.UserUpdatePolicyName);

			if (authorizationResult.Succeeded == false) 
				return new ForbidResult();

			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}

	
		[HttpGet("{id}")]
		public async Task<ActionResult<UserDetailsDto>> GetById(
			Guid id, CancellationToken cancellationToken)
		{
			var query = new GetUserQuery(id);
			var response = await _mediator.Send(query, cancellationToken);

			return Ok(response);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
		{
			var authorizationResult = await _authorizationService
				.AuthorizeAsync(User, id, Policies.UserDeletePolicyName);

			if (authorizationResult.Succeeded == false)
				return new ForbidResult();

			var command = new DeleteUserCommand(id);
			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}

		[HttpGet("{id}/avatar")]
		public async Task<ActionResult> GetAvatar(Guid id, CancellationToken cancellationToken)
		{
			var query = new GetUserAvatarQuery(id);
			var response = await _mediator.Send(query, cancellationToken);

			return File(response.Bytes, response.ContentType, response.FileName);
		}

		[HttpPut("{id}/avatar")]
		[Authorize]
		public async Task<ActionResult> UpdateAvatar(
			Guid id, [FromForm] IFormFile avatarFile, CancellationToken cancellationToken)
		{
			var authorizationResult = await _authorizationService
				.AuthorizeAsync(User, id, Policies.UserUpdatePolicyName);

			if (authorizationResult.Succeeded == false)
				return new ForbidResult();

			var command = new UpdateUserAvatarCommand(id, avatarFile);
			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}

		[HttpDelete("{id}/avatar")]
		[Authorize]
		public async Task<ActionResult> DeleteAvatar(
			Guid id, CancellationToken cancellationToken)
		{
			var authorizationResult = await _authorizationService
				.AuthorizeAsync(User, id, Policies.UserDeletePolicyName);

			if (authorizationResult.Succeeded == false)
				return new ForbidResult();

			var command = new DeleteUserAvatarCommand(id);
			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}

		[HttpGet("{id}/reviews")]
		public async Task<ActionResult<PagedList<ReviewDto>>> GetReviews(
			Guid id, int page, int size, CancellationToken cancellationToken)
		{
			var query = new GetUserReviewsQuery(id, new Page(page, size));
			var resopnse = await _mediator.Send(query, cancellationToken);

			return Ok(resopnse);
		}

		[HttpGet("{id}/read-books")]
		public async Task<ActionResult<PagedList<BookLookupDto>>> GetReadBooks(
			Guid id, int page, int size, CancellationToken cancellationToken)
		{
			var query = new GetUserReadBooksQuery(id, new Page(page, size));
			var response = await _mediator.Send(query, cancellationToken);

			return Ok(response);
		}

		[HttpPost("{userId}/read-books/{bookId}")]
		[Authorize]
		public async Task<ActionResult> AddReadBook(
			Guid userId, Guid bookId, CancellationToken cancellationToken)
		{
			var authorizationResult = await _authorizationService
				.AuthorizeAsync(User, userId, Policies.UserUpdatePolicyName);

			if (authorizationResult.Succeeded == false)
				return new ForbidResult();

			var command = new AddReadBookCommand(userId, bookId);
			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}

		[HttpDelete("{userId}/read-books/{bookId}")]
		[Authorize]
		public async Task<ActionResult> DeleteReadedBook(
			Guid userId, Guid bookId, CancellationToken cancellationToken)
		{
			var authorizationResult = await _authorizationService
				.AuthorizeAsync(User, userId, Policies.UserUpdatePolicyName);

			if (authorizationResult.Succeeded == false)
				return new ForbidResult();

			var command = new DeleteReadBookCommand(userId, bookId);
			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}
	}
}
