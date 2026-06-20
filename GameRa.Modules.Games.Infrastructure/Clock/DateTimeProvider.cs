using GameRa.Modules.Games.Application.Abstractions.Clock;

namespace GameRa.Modules.Games.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
