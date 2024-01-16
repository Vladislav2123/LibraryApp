using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json;
using FileNotFoundException = LibraryApp.Domain.Exceptions.FileNotFoundException;
using LibraryApp.Domain.Exceptions;

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
			if (ShouldLog(exception)) Log.Error(exception.Message);
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

	/// <summary>
	/// Returns response status code depends on exception.
	/// </summary>
	private int GetStatusCode(Exception exception) => exception switch
	{
		LoginFailedException => StatusCodes.Status400BadRequest,
		UserHasNotReadBookException => StatusCodes.Status400BadRequest,
		EntityAlreadyExistException => StatusCodes.Status400BadRequest,
		BookAlreadyHasReviewException => StatusCodes.Status400BadRequest,
		EmailAlreadyInUseException => StatusCodes.Status400BadRequest,
		UserAlreadyReadBookException => StatusCodes.Status400BadRequest,
		ValidationException => StatusCodes.Status400BadRequest,
		EntityHasNoChangesException => StatusCodes.Status400BadRequest,
		SecurityTokenException => StatusCodes.Status401Unauthorized,
		EntityNotFoundException => StatusCodes.Status404NotFound,
		FileNotFoundException => StatusCodes.Status404NotFound,
		ContentTypeNotFoundException => StatusCodes.Status500InternalServerError,
		_ => StatusCodes.Status500InternalServerError
	};


	/// <summary>
	/// Returns true if exception must be logged.
	/// </summary>
	private bool ShouldLog(Exception exception) => exception switch
	{
		SecurityTokenException => false,
		_ => true
	};

	private IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
	{
		IReadOnlyDictionary<string, string[]> errors = null;

		if (exception is ValidationException validationException)
		{
			errors = validationException.ErrorsDictionary;
		}

		return errors;
	}
}

