using LibraryApp.Application.Common.Exceptions;
using Serilog;
using System.Text.Json;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
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
				ValidationException => StatusCodes.Status422UnprocessableEntity,
				EntityHasNoChangesException => StatusCodes.Status422UnprocessableEntity,
				EntityAlreadyExistException => StatusCodes.Status409Conflict,
				BookAlreadyHasReviewException => StatusCodes.Status409Conflict,
				UserEmailAlreadyUsingException => StatusCodes.Status409Conflict,
				UserAlreadyReadBookException => StatusCodes.Status409Conflict,
				UserHasNotReadBookException => StatusCodes.Status400BadRequest,
				EntityNotFoundException => StatusCodes.Status404NotFound,
				FileNotFoundException => StatusCodes.Status404NotFound,
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
}

