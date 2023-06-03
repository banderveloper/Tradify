using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Tradify.Identity.Application.Common.Extensions;

public static class ValidationResultExtensions
{
    public static ValidationProblemDetails ToProblemDetails(this ValidationResult validationResult)
    {
        var error = new ValidationProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = 400,
        };

        foreach (var validationFailure in validationResult.Errors)
        {
            if (error.Errors.ContainsKey(validationFailure.PropertyName))
            {
                error.Errors[validationFailure.PropertyName]
                    = error.Errors[validationFailure.PropertyName]
                        .Concat(new[] { validationFailure.ErrorMessage }).ToArray();
                continue;
            }
            
            error.Errors.Add(new KeyValuePair<string, string[]>(
                validationFailure.PropertyName,
                new []{validationFailure.ErrorMessage}));
        }

        return error;
    }
}
