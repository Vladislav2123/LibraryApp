using LibraryApp.Application.Common.Exceptions;
using System.Text.Json;

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

