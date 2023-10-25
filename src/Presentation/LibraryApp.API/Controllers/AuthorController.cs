using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Feauters.Authors.Commands.Create;
using LibraryApp.Application.Feauters.Authors.Commands.Delete;
using LibraryApp.Application.Feauters.Authors.Commands.Update;
using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using LibraryApp.Application.Feauters.Authors.Queries.GetAuthor;
using LibraryApp.Application.Feauters.Authors.Queries.GetAuthors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.API.Controllers
{
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
		public async Task<ActionResult<PagedList<AuthorLookupDto>>> Get(string? search, int page, int limit)
		{
			var query = new GetAuthorsQuery(search, page, limit);
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<AuthorDto>> GetById(Guid id)
		{
			var query = new GetAuthorQuery(id);
			var response = await _mediator.Send(query);

			return Ok(response);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create([FromBody] CreateAuthorCommand command)
		{
			var response = await _mediator.Send(command);

			return CreatedAtAction(nameof(Create), response);
		}

		[HttpPut]
		public async Task<ActionResult> Update([FromBody] UpdateAuthorCommand command)
		{
			await _mediator.Send(command);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(Guid id)
		{
			var command = new DeleteAuthorCommand(id);
			await _mediator.Send(command);

			return NoContent();
		}
	}
}
