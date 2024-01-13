using FluentValidation;
using MediatR;
using ValidationException = LibraryApp.Domain.Exceptions.ValidationException;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Behaviours;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any() == false) return await next();

        var context = new ValidationContext<TRequest>(request);

        var failuresDictionary = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .GroupBy(
            failure => failure.PropertyName,
            failure => failure.ErrorMessage,
            (propertyName, errorMessages) => new
            {
                Key = propertyName,
                Values = errorMessages.Distinct().ToArray()
            })
            .ToDictionary(x => x.Key, x => x.Values);

        if (failuresDictionary.Any())
        {
            throw new ValidationException(request.GetType().Name, failuresDictionary);
        }

        return await next();
    }
}
