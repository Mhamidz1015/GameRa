using Bogus;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Distribution.Domain.GameVersions;

namespace GameRa.Modules.Distribution.UnitTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Faker Faker = new();

    public static T AssertDomainEventWasPublished<T>(Entity entity)
        where T : IDomainEvent
    {
        T? domainEvent = entity.DomainEvents.OfType<T>().SingleOrDefault();

        if (domainEvent is null)
        {
            throw new Exception($"{typeof(T).Name} was not published");
        }

        return domainEvent;
    }

    protected static VersionNumber Version(string value)
    {
        if (!VersionNumber.TryParse(value, out VersionNumber version))
        {
            throw new ArgumentException($"Invalid test version '{value}'", nameof(value));
        }

        return version;
    }

    protected static string Sha256() => Faker.Random.Hexadecimal(64, string.Empty).ToLowerInvariant();
}
