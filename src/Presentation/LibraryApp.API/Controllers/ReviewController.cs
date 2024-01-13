using LibraryApp.API.Authorization;
using LibraryApp.Application.Feauters.Reviews.Commands.Create;
using LibraryApp.Application.Feauters.Reviews.Commands.Delete;
using LibraryApp.Application.Feauters.Reviews.Commands.Update;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Feauters.Reviews.Queries.GetAllReviews;
using LibraryApp.Application.Pagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly IAuthorizationService _authorizationService;

	public ReviewController(IMediator mediator, IAuthorizationService authorizationService)
	{

		_mediator = mediator;
		_authorizationService = authorizationService;
	}

	[HttpGet]
	public async Task<ActionResult<PagedList<ReviewDto>>> GetAll(
		int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetAllReviewsQuery(new Page(page, size));
		var response = await _mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	[HttpPost]
	[Authorize]
	public async Task<ActionResult<Guid>> Create(
		[FromBody] CreateReviewCommand command, CancellationToken cancellationToken)
	{
		var response = await _mediator.Send(command, cancellationToken);

		return CreatedAtAction(nameof(Create), response);
	}

	[HttpPut]
	[Authorize]
	public async Task<ActionResult> Update
		([FromBody] UpdateReviewCommand command, CancellationToken cancellationToken)
	{
		var authorizationResult = await _authorizationService
			.AuthorizeAsync(User, command.ReviewId, Policies.ReviewUpdatePolicyName);

		if (authorizationResult.Succeeded == false)
			return new ForbidResult();

            await _mediator.Send(command, cancellationToken);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[Authorize]
	public async Task<ActionResult> Delete(
		Guid id, CancellationToken cancellationToken)
	{
		var authorizationResult = await _authorizationService
			.AuthorizeAsync(User, id, Policies.ReviewDeletePolicyName);

		if (authorizationResult.Succeeded == false)
			return new ForbidResult();

		var command = new DeleteReviewCommand(id);
		await _mediator.Send(command, cancellationToken);

		return NoContent();
	}
}
