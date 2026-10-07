using FluentValidation;
using Microsoft.Extensions.Options;

namespace GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrls;

internal sealed class GetFileDownloadUrlsQueryValidator : AbstractValidator<GetFileDownloadUrlsQuery>
{
    public GetFileDownloadUrlsQueryValidator(IOptions<DistributionOptions> options)
    {
        RuleFor(q => q.UserId).NotEmpty();
        RuleFor(q => q.FileIds)
            .NotEmpty()
            .Must(ids => ids.Count <= options.Value.MaxBatchDownloadUrls)
            .WithMessage($"At most {options.Value.MaxBatchDownloadUrls} download URLs can be requested at once");
        RuleForEach(q => q.FileIds).NotEmpty();
    }
}
