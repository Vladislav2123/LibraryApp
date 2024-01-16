using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

	protected IMediator Mediator { get; }

	public ControllerBase(IMediator mediator, IAuthorizationService authorizationService)
	{
		Mediator = mediator;
		_authorizationService = authorizationService;
	}

	protected async Task<bool> AuthorizeAsync(ClaimsPrincipal user, string policyName)
	{
		var authorizatinResult = await _authorizationService.AuthorizeAsync(user, policyName);
		return authorizatinResult.Succeeded;
	}

	protected async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
	{
		var authorizationResult = await _authorizationService.AuthorizeAsync(user, resource, policyName);
		return authorizationResult.Succeeded; 
	}
}
