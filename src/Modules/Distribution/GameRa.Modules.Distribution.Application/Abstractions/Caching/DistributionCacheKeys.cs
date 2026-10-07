namespace GameRa.Modules.Distribution.Application.Abstractions.Caching;

internal static class DistributionCacheKeys
{
    internal static readonly TimeSpan LatestVersionExpiration = TimeSpan.FromMinutes(5);

    internal static string LatestVersion(Guid gameId) => $"distribution:latest:{gameId}";
}
