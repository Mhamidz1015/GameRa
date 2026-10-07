using FluentValidation;
using FluentValidation.Results;
using GameRa.Common.Domain.Abstractions;

namespace GameRa.Modules.Distribution.Application.Abstractions.Validation;

// The shared validation pipeline only covers commands, so query handlers run their validator explicitly.
internal static class ValidatorExtensions
{
    internal static async Task<ValidationError?> ValidateRequestAsync<T>(
        this IValidator<T> validator,
        T request,
        CancellationToken cancellationToken)
    {
        ValidationResult result = await validator.ValidateAsync(request, cancellationToken);

        return result.IsValid
            ? null
            : new ValidationError(result.Errors.Select(f => Error.Problem(f.ErrorCode, f.ErrorMessage)).ToArray());
    }
}
