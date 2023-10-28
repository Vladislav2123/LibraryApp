using LibraryApp.Application.Feauters.Reviews.Commands.Create;
using LibraryApp.Application.Feauters.Reviews.Commands.Delete;
using LibraryApp.Application.Feauters.Reviews.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
    [ApiController]
	[Route("api/reviews")]
	public class ReviewController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ReviewController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost()]
		public async Task<ActionResult<Guid>> Create([FromBody] CreateReviewCommand command)
		{
			var response = await _mediator.Send(command);

			return CreatedAtAction(nameof(Create), response);
		}

		[HttpPut()]
		public async Task<ActionResult> Update([FromBody] UpdateReviewCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}

		[HttpDelete()]
		public async Task<ActionResult> Delete(DeleteReviewCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}
	}
}
