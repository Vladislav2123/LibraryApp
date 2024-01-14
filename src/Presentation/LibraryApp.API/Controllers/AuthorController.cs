using LibraryApp.API.Authorization;
using LibraryApp.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibraryApp.Application.Features.Authors.Commands.Create;
using LibraryApp.Application.Features.Authors.Commands.Delete;
using LibraryApp.Application.Features.Authors.Commands.DeleteAuthorAvatar;
using LibraryApp.Application.Features.Authors.Commands.Update;
using LibraryApp.Application.Features.Authors.Commands.UpdateAuthorAvatar;
using LibraryApp.Application.Features.Authors.Queries.Dto;
using LibraryApp.Application.Features.Authors.Queries.GetAllAuthors;
using LibraryApp.Application.Features.Authors.Queries.GetAuthor;
using LibraryApp.Application.Features.Authors.Queries.GetAuthorAvatar;

namespace LibraryApp.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorController : ControllerBase
{
	private readonly IMediator _mediator;

	public AuthorController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<ActionResult<PagedList<AuthorLookupDto>>> GetAll(
		string? search, int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetAllAuthorsQuery(search, new Page(page, size));
		var response = await _mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpPost]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult<Guid>> Create(
		[FromBody] CreateAuthorCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return CreatedAtAction(nameof(Create), response);
	}

	[HttpPut]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult> Update(
		[FromBody] UpdateAuthorCommand command, CancellationToken cancellationToken)
	{
		await _mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		var command = new DeleteAuthorCommand(id);
		await _mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<AuthorDto>> GetById(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetAuthorQuery(id);
		var response = await _mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpGet("{id}/avatar")]
	public async Task<ActionResult> GetAvatar(Guid id, CancellationToken cancellationToken)
	{
		var query = new GetAuthorAvatarQuery(id);
		var response = await _mediator.Send(query, cancellationToken);

		return File(response.Bytes, response.ContentType, response.FileName);
	}

	[HttpPut("{id}/avatar")]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult> UpdateAvatar(
		Guid id, [FromForm] IFormFile avatarFile, CancellationToken cancellationToken)
	{
		var command = new UpdateAuthorAvatarCommand(id, avatarFile);
		await _mediator.Send(command, cancellationToken);

		return NoContent();
	}
	
	[HttpDelete("{id}/avatar")]
	[Authorize(Policy = Policies.AdminOnlyPolicyName)]
	public async Task<ActionResult> DeleteAvatar(Guid id, CancellationToken cancellationToken)
	{
		var command = new DeleteAuthorAvatarCommand(id);
		await _mediator.Send(command, cancellationToken);

		return NoContent();
	}
}
