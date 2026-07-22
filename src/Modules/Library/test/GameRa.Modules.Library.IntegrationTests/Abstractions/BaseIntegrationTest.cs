using Bogus;
using GameRa.Modules.Library.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameRa.Modules.Library.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    protected static readonly Faker Faker = new();
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly LibraryItemDbContext DbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<LibraryItemDbContext>();
    }

    protected async Task CleanDatabaseAsync()
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM libraryitem.inbox_message_consumers;
            DELETE FROM libraryitem.inbox_messages;
            DELETE FROM libraryitem.outbox_message_consumers;
            DELETE FROM libraryitem.outbox_messages;
            DELETE FROM libraryitem.libraryitem;
            """);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
