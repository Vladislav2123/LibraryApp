﻿using LibraryApp.Application.Common.Pagination;
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
		public async Task<ActionResult<PagedList<AuthorLookupDto>>> GetAll(
			string? search, int page, int size, CancellationToken cancellationToken)
		{
			var query = new GetAllAuthorsQuery(search, new Page(page, size));
			var response = await _mediator.Send(query, cancellationToken);

			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<AuthorDto>> GetById(Guid id, CancellationToken cancellationToken)
		{
			var query = new GetAuthorQuery(id);
			var response = await _mediator.Send(query, cancellationToken);

			return Ok(response);
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> Create(
			[FromBody] CreateAuthorCommand command, CancellationToken cancellationToken)
		{
			var response = await _mediator.Send(command, cancellationToken);

			return CreatedAtAction(nameof(Create), response);
		}

		[HttpPut]
		public async Task<ActionResult> Update(
			[FromBody] UpdateAuthorCommand command, CancellationToken cancellationToken)
		{
			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}

		[HttpDelete()]
		public async Task<ActionResult> Delete(DeleteAuthorCommand command, CancellationToken cancellationToken)
		{
			await _mediator.Send(command, cancellationToken);

			return NoContent();
		}
	}
}
