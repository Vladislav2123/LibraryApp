using LibraryApp.Application.Features.Books.Querries.GetUserReadBooks;
using LibraryApp.Application.Features.Users.Commands.DeleteUserAvatar;
using LibraryApp.Application.Features.Users.Commands.UpdateUserAvatar;
using LibraryApp.Application.Features.Reviews.Queries.GetUserReviews;
using LibraryApp.Application.Features.Users.Commands.UpdateUserRole;
using LibraryApp.Application.Features.Users.Commands.DeleteReadBook;
using LibraryApp.Application.Features.Users.Queries.GetUserAvatar;
using LibraryApp.Application.Features.Users.Commands.AddReadBook;
using LibraryApp.Application.Features.Users.Queries.GetAllUsers;
using LibraryApp.Application.Features.Users.Queries.GetUser;
using LibraryApp.Application.Features.Users.Commands.Create;
using LibraryApp.Application.Features.Users.Commands.Update;
using LibraryApp.Application.Features.Users.Commands.Delete;
using LibraryApp.Application.Features.Users.Queries.Login;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using LibraryApp.Application.Features.Books.Querries.Dto;
using LibraryApp.Application.Features.Users.Queries.Dto;
using LibraryApp.Application.Pagination;
using Microsoft.AspNetCore.Authorization;
using LibraryApp.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LibraryApp.API.Controllers;
[Route("api/users")]
public class UserController : ControllerBase
{
	public UserController(IMediator mediator, IAuthorizationService authorizationService)
		: base(mediator, authorizationService) { }

	[HttpGet("login")]
	[AllowAnonymous]
	public async Task<ActionResult<LoginResponse>> Login(
		LoginQuery command, CancellationToken cancellationToken)
	{
		var response = await Mediator.Send(command, cancellationToken);

		return Ok(response);
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ActionResult<Guid>> Register(
		[FromBody] CreateUserCommand command, CancellationToken cancellationToken)
	{
		var response = await Mediator.Send(command, cancellationToken);

		return CreatedAtAction(nameof(Register), response);
	}

	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<PagedList<UserLookupDto>>> GetAll(
		string? search, string? sortColumn, string? sortOrder,
		int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetAllUsersQuery(search, sortColumn, sortOrder, new Page(page, size));
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpPut]
	public async Task<ActionResult> Update(
		[FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, command.UserId, Policies.UserUpdatePolicyName) == false) 
			return Forbid();

		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}


	[HttpGet("{id}")]
	public async Task<ActionResult<UserDetailsDto>> GetById(
		Guid id, CancellationToken cancellationToken)
	{
		var query = new GetUserQuery(id);
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, id, Policies.UserDeletePolicyName) == false)
			return Forbid();

		var command = new DeleteUserCommand(id);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpGet("{id}/avatar")]
	[AllowAnonymous]
	public async Task<ActionResult> GetAvatar(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetUserAvatarQuery(id);
		var response = await Mediator.Send(query, cancellationToken);

		return File(response.Bytes, response.ContentType, response.FileName);
	}

	[HttpPut("{id}/avatar")]
	public async Task<ActionResult> UpdateAvatar(
		Guid id, [FromForm] IFormFile avatarFile, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, id, Policies.UserUpdatePolicyName) == false)
			return Forbid();

		var command = new UpdateUserAvatarCommand(id, avatarFile);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{id}/avatar")]
	public async Task<ActionResult> DeleteAvatar(
		Guid id, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, id, Policies.UserDeletePolicyName) == false)
			return Forbid();

		var command = new DeleteUserAvatarCommand(id);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpGet("{id}/reviews")]
	[AllowAnonymous]
	public async Task<ActionResult<PagedList<ReviewDto>>> GetReviews(
		Guid id, int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetUserReviewsQuery(id, new Page(page, size));
		var resopnse = await Mediator.Send(query, cancellationToken);

		return Ok(resopnse);
	}

	[HttpGet("{id}/read-books")]
	[AllowAnonymous]
	public async Task<ActionResult<PagedList<BookLookupDto>>> GetReadBooks(
		Guid id, int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetUserReadBooksQuery(id, new Page(page, size));
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpPost("{userId}/read-books/{bookId}")]
	public async Task<ActionResult> AddReadBook(
		Guid userId, Guid bookId, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, userId, Policies.UserUpdatePolicyName) == false)
			return Forbid();

		var command = new AddReadBookCommand(userId, bookId);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{userId}/read-books/{bookId}")]
	public async Task<ActionResult> DeleteReadedBook(
		Guid userId, Guid bookId, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, userId, Policies.UserUpdatePolicyName) == false)
			return Forbid();

		var command = new DeleteReadBookCommand(userId, bookId);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpPut("{userId}/role")]
	public async Task<ActionResult> UpdateUserRole(
		Guid userId, [FromForm] string role, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var command = new UpdateUserRoleCommand(userId, role);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}
}
