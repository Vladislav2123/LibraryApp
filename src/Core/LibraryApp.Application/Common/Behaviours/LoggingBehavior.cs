using MediatR;
using Serilog;

namespace LibraryApp.Application.Common.Behaviours
{
	public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			DateTime startTime = DateTime.UtcNow;

			Log.Information(
				"Starting request {@RequestName}",
				typeof(TRequest).Name);

			var result = await next();

			Log.Information(
				"Request {@RequestName} complete ({@Time} ms)",
				typeof(TRequest).Name,
				DateTime.UtcNow.Subtract(startTime).TotalMilliseconds);

			return result;
		}
	}
}
