using LibraryApp.Application.Common.Helpers.Pagination;
using LibraryApp.Application.Feauters.Reviews.Commands.Create;
using LibraryApp.Application.Feauters.Reviews.Commands.Delete;
using LibraryApp.Application.Feauters.Reviews.Commands.Update;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Feauters.Reviews.Queries.GetBookReviews;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [ApiController]
	[Route("api/books")]
	public class ReviewController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ReviewController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("{id}/reviews")]
		public async Task<ActionResult<PagedList<ReviewDto>>> Get(
			Guid id, string? sortColumn, string? sortOrder, int page, int size)
		{
			var query = new GetBookReviewsQuery(id, sortColumn, sortOrder, new Page(page, size));
			var response = _mediator.Send(query);

			return Ok(response);
		}

		[HttpPost("{id}/reviews")]
		public async Task<ActionResult<Guid>> Create([FromBody] CreateReviewCommand command)
		{
			var response = await _mediator.Send(command);

			return CreatedAtAction(nameof(Create), response);
		}

		[HttpPut("{id}/reviews")]
		public async Task<ActionResult> Update([FromBody] UpdateReviewCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}

		[HttpDelete("{bookId}/reviews/{reviewId}")]
		public async Task<ActionResult> Delete(Guid reviewId)
		{
			var command = new DeleteReviewCommand(reviewId);
			await _mediator.Send(command);

			return NoContent();
		}
	}
}
