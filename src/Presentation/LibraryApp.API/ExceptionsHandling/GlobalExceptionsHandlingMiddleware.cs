using LibraryApp.Application.Common.Exceptions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.API.ExceptionsHandling;

    public class GlobalExceptionsHandlingMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception exception)
		{
			Log.Error(exception.Message);
			await HandleExceptionAsync(context, exception);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		int statusCode = GetStatusCode(exception);

		var response = new
		{
			status = statusCode,
			message = exception.Message,
			errors = GetErrors(exception)
		};


		context.Response.StatusCode = statusCode;
		context.Response.ContentType = "application/json";

		await context.Response.WriteAsync(JsonSerializer.Serialize(response));
	}

	private int GetStatusCode(Exception exception) =>
		exception switch
		{
			LoginFailedException => StatusCodes.Status400BadRequest,
			UserHasNotReadBookException => StatusCodes.Status400BadRequest,
			EntityNotFoundException => StatusCodes.Status404NotFound,
			FileNotFoundException => StatusCodes.Status404NotFound,
			SecurityTokenException => StatusCodes.Status401Unauthorized,
			EntityAlreadyExistException => StatusCodes.Status409Conflict,
			BookAlreadyHasReviewException => StatusCodes.Status409Conflict,
			EmailAlreadyInUseException => StatusCodes.Status409Conflict,
			UserAlreadyReadBookException => StatusCodes.Status409Conflict,
			ValidationException => StatusCodes.Status422UnprocessableEntity,
			EntityHasNoChangesException => StatusCodes.Status422UnprocessableEntity,
			ContentTypeNotFoundException => StatusCodes.Status500InternalServerError,
			_ => StatusCodes.Status500InternalServerError
		};

	private IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
	{
		IReadOnlyDictionary<string, string[]> errors = null;

		if(exception is ValidationException validationException)
		{
			errors = validationException.ErrorsDictionary;
		}

		return errors;
	}
}

