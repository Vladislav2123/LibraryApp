using LibraryApp.Application.Features.Authors.Commands.DeleteAuthorAvatar;
using LibraryApp.Application.Features.Authors.Commands.UpdateAuthorAvatar;
using LibraryApp.Application.Features.Authors.Queries.GetAuthorAvatar;
using LibraryApp.Application.Features.Authors.Queries.GetAllAuthors;
using LibraryApp.Application.Features.Authors.Queries.GetAuthor;
using LibraryApp.Application.Features.Authors.Commands.Create;
using LibraryApp.Application.Features.Authors.Commands.Update;
using LibraryApp.Application.Features.Authors.Commands.Delete;
using LibraryApp.Application.Features.Authors.Queries.Dto;
using Microsoft.AspNetCore.Authorization;
using LibraryApp.Application.Pagination;
using LibraryApp.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LibraryApp.API.Controllers;
[Route("api/authors")]
public class AuthorController : ControllerBase
{
	public AuthorController(IMediator mediator, IAuthorizationService authorizationService)
		: base(mediator, authorizationService) { }

	// Get all Authors
	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<PagedList<AuthorLookupDto>>> GetAll(
		string? search, int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetAllAuthorsQuery(search, new Page(page, size));
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	// Create new Author
	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		[FromBody] CreateAuthorCommand command, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var response = await Mediator.Send(command, cancellationToken);

		return CreatedAtAction(nameof(Create), response);
	}

	// Update Author
	[HttpPut]
	public async Task<ActionResult> Update(
		[FromBody] UpdateAuthorCommand command, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	// Delete Author
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var command = new DeleteAuthorCommand(id);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	// Get Author by Id
	[HttpGet("{id}")]
	[AllowAnonymous]
	public async Task<ActionResult<AuthorDto>> GetById(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetAuthorQuery(id);
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	// Get Author`s avatar 
	[HttpGet("{id}/avatar")]
	[AllowAnonymous]
	public async Task<ActionResult> GetAvatar(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetAuthorAvatarQuery(id);
		var response = await Mediator.Send(query, cancellationToken);

		return File(response.Bytes, response.ContentType, response.FileName);
	}

	// Update Author`s avatar
	[HttpPut("{id}/avatar")]
	public async Task<ActionResult> UpdateAvatar(
		Guid id, [FromForm] IFormFile avatarFile, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var command = new UpdateAuthorAvatarCommand(id, avatarFile);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}
	
	// Delete Author`s avatar
	[HttpDelete("{id}/avatar")]
	public async Task<ActionResult> DeleteAvatar(Guid id, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, Policies.AdminOnlyPolicyName) == false)
			return Forbid();

		var command = new DeleteAuthorAvatarCommand(id);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}
}
