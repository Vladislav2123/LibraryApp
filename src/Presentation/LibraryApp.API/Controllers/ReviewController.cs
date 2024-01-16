using LibraryApp.Application.Features.Reviews.Queries.GetAllReviews;
using LibraryApp.Application.Features.Reviews.Commands.Create;
using LibraryApp.Application.Features.Reviews.Commands.Delete;
using LibraryApp.Application.Features.Reviews.Commands.Update;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using Microsoft.AspNetCore.Authorization;
using LibraryApp.Application.Pagination;
using LibraryApp.API.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace LibraryApp.API.Controllers;
[Route("api/reviews")]
public class ReviewController : ControllerBase
{
	public ReviewController(IMediator mediator, IAuthorizationService authorizationService)
		: base(mediator, authorizationService) { }

	// Get all Reviews
	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<PagedList<ReviewDto>>> GetAll(
		int page, int size, CancellationToken cancellationToken)
	{
		var query = new GetAllReviewsQuery(new Page(page, size));
		var response = await Mediator.Send(query, cancellationToken);

		return Ok(response);
	}

	// Create new Review
	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		[FromBody] CreateReviewCommand command, CancellationToken cancellationToken)
	{
		var response = await Mediator.Send(command, cancellationToken);

		return CreatedAtAction(nameof(Create), response);
	}

	// Update Review
	[HttpPut]
	public async Task<ActionResult> Update
		([FromBody] UpdateReviewCommand command, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, command.ReviewId, Policies.ReviewUpdatePolicyName) == false)
			return Forbid();

		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}

	// Delete Review
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(
		Guid id, CancellationToken cancellationToken)
	{
		if (await AuthorizeAsync(User, id, Policies.ReviewDeletePolicyName) == false)
			return Forbid();

		var command = new DeleteReviewCommand(id);
		await Mediator.Send(command, cancellationToken);

		return NoContent();
	}
}
