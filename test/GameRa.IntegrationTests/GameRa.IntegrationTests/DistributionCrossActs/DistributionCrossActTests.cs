using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.IntegrationTests.Abstractions;
using GameRa.Modules.Distribution.Application.Downloads;
using GameRa.Modules.Distribution.Application.Downloads.GetFileDownloadUrl;
using GameRa.Modules.Distribution.Application.Games.AddDistributionGame;
using GameRa.Modules.Distribution.Application.GameVersions.ConfirmVersionFileUpload;
using GameRa.Modules.Distribution.Application.GameVersions.CreateGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.MarkVersionReady;
using GameRa.Modules.Distribution.Application.GameVersions.PublishGameVersion;
using GameRa.Modules.Distribution.Application.GameVersions.RegisterVersionFile;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.IntegrationEvents;
using GameRa.Modules.Library.Application.LibraryItems.ArchivedLibraryItem;
using GameRa.Modules.Library.Application.LibraryItems.RevokeGameFromAllLibraries;
using GameRa.Modules.Store.Application.Carts.AddItemToCart;
using GameRa.Modules.Store.Application.Orders.RefundOrder;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using GamesDelistGameCommand = GameRa.Modules.Games.Application.Games.DelistGame.DelistGameCommand;

namespace GameRa.IntegrationTests.DistributionCrossActs;

public sealed class DistributionCrossActTests : BaseIntegrationTest
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

    private readonly IntegrationTestWebAppFactory _factory;
    private readonly FakeFileStorage _fileStorage;

    public DistributionCrossActTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
        _fileStorage = factory.Services.GetRequiredService<FakeFileStorage>();
    }

    [Fact]
    public async Task Purchase_ShouldAllowDownload_OnlyAfterLibraryGrantsOwnership()
    {
        (Guid gameId, Guid fileId) = await CreateStoreGameWithPublishedVersionAsync();
        Guid customerId = await CreateCustomerAsync();

        Result<FileDownloadUrlResponse> beforePurchase = await Sender.Send(new GetFileDownloadUrlQuery(customerId, fileId));
        beforePurchase.Error.Should().Be(DistributionErrors.GameNotOwned(gameId));

        await PurchaseAsync(customerId, gameId);

        (await WaitUntilDownloadableAsync(customerId, fileId)).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Refund_ShouldRevokeDownloadAccess()
    {
        (Guid gameId, Guid fileId) = await CreateStoreGameWithPublishedVersionAsync();
        Guid customerId = await CreateCustomerAsync();
        Guid orderId = await PurchaseAsync(customerId, gameId);
        (await WaitUntilDownloadableAsync(customerId, fileId)).IsSuccess.Should().BeTrue();

        (await Sender.Send(new RefundOrderCommand(orderId, customerId))).IsSuccess.Should().BeTrue();

        (await WaitUntilForbiddenAsync(customerId, gameId, fileId)).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task AdminRevoke_ShouldRevokeDownloadAccess()
    {
        (Guid gameId, Guid fileId) = await CreateStoreGameWithPublishedVersionAsync();
        Guid customerId = await CreateCustomerAsync();
        await PurchaseAsync(customerId, gameId);
        (await WaitUntilDownloadableAsync(customerId, fileId)).IsSuccess.Should().BeTrue();

        (await Sender.Send(new RevokeGameFromAllLibrariesCommand(gameId))).IsSuccess.Should().BeTrue();

        (await WaitUntilForbiddenAsync(customerId, gameId, fileId)).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Archive_ShouldNotAffectDownloadAccess()
    {
        (Guid gameId, Guid fileId) = await CreateStoreGameWithPublishedVersionAsync();
        Guid customerId = await CreateCustomerAsync();
        await PurchaseAsync(customerId, gameId);
        (await WaitUntilDownloadableAsync(customerId, fileId)).IsSuccess.Should().BeTrue();

        (await Sender.Send(new ArchivedLibraryItemCommand(customerId, gameId))).IsSuccess.Should().BeTrue();
        await Task.Delay(TimeSpan.FromSeconds(3));

        (await Sender.Send(new GetFileDownloadUrlQuery(customerId, fileId))).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Delist_ShouldKeepOwnerDownloads_AndBlockNewPurchases()
    {
        Guid gameId = await Sender.CreateGameInGamesModuleAsync();
        Guid fileId = await WaitAndPublishVersionAsync(gameId);
        Guid owner = await CreateCustomerAsync();
        Result<bool> addedToCart = await Poller.WaitAsync(Timeout, async () =>
        {
            Result add = await Sender.Send(new AddItemToCartCommand(owner, gameId));
            return add.IsSuccess ? Result.Success(true) : Result.Failure<bool>(add.Error);
        });
        addedToCart.IsSuccess.Should().BeTrue();
        await Sender.CreateOrderAsync(owner);
        (await WaitUntilDownloadableAsync(owner, fileId)).IsSuccess.Should().BeTrue();

        (await Sender.Send(new GamesDelistGameCommand(gameId))).IsSuccess.Should().BeTrue();

        Guid newBuyer = await CreateCustomerAsync();
        Result<bool> purchaseBlocked = await Poller.WaitAsync(Timeout, async () =>
        {
            // A fresh scope per attempt, so Store's game is not served from this test's change tracker.
            using IServiceScope scope = _factory.Services.CreateScope();
            Result add = await scope.ServiceProvider.GetRequiredService<MediatR.ISender>()
                .Send(new AddItemToCartCommand(newBuyer, gameId));
            return add.IsFailure ? Result.Success(true) : Result.Failure<bool>(Error.Failure("Game.NotDelisted", "Not yet"));
        });
        purchaseBlocked.IsSuccess.Should().BeTrue();
        (await Sender.Send(new GetFileDownloadUrlQuery(owner, fileId))).IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GameAddedInGames_ShouldAllowCreatingVersions()
    {
        Guid gameId = await Sender.CreateGameInGamesModuleAsync();

        Result<Guid> result = await Poller.WaitAsync(Timeout, async () =>
            await Sender.Send(new CreateGameVersionCommand(gameId, "1.0.0", null)));

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Publish_ShouldPublishGameVersionPublishedIntegrationEvent()
    {
        var gameId = Guid.NewGuid();
        (await Sender.Send(new AddDistributionGameCommand(gameId))).IsSuccess.Should().BeTrue();

        var received = new TaskCompletionSource<GameVersionPublishedIntegrationEvent>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        IReceiveEndpointConnector connector = _factory.Services.GetRequiredService<IReceiveEndpointConnector>();
        HostReceiveEndpointHandle handle = connector.ConnectReceiveEndpoint($"distribution-test-{gameId:N}", (_, cfg) =>
            cfg.Handler<GameVersionPublishedIntegrationEvent>(context =>
            {
                if (context.Message.GameId == gameId)
                {
                    received.TrySetResult(context.Message);
                }

                return Task.CompletedTask;
            }));
        await handle.Ready;

        try
        {
            (Guid versionId, _) = await PublishVersionAsync(gameId, "2.1.0");

            GameVersionPublishedIntegrationEvent integrationEvent = await received.Task.WaitAsync(Timeout);

            integrationEvent.VersionId.Should().Be(versionId);
            integrationEvent.Version.Should().Be("2.1.0");
        }
        finally
        {
            await handle.StopAsync();
        }
    }

    private async Task<Guid> CreateCustomerAsync()
    {
        Guid customerId = Faker.Random.Guid();
        await Sender.CreateCustomerAsync(customerId);
        return customerId;
    }

    private async Task<Guid> PurchaseAsync(Guid customerId, Guid gameId)
    {
        await Sender.AddItemToCartAsync(customerId, gameId);
        return await Sender.CreateOrderAsync(customerId);
    }

    private async Task<(Guid GameId, Guid FileId)> CreateStoreGameWithPublishedVersionAsync()
    {
        Guid gameId = Faker.Random.Guid();
        await Sender.AddGameAsync(gameId);
        (await Sender.Send(new AddDistributionGameCommand(gameId))).IsSuccess.Should().BeTrue();
        (_, Guid fileId) = await PublishVersionAsync(gameId, "1.0.0");
        return (gameId, fileId);
    }

    private async Task<Guid> WaitAndPublishVersionAsync(Guid gameId)
    {
        Result<Guid> created = await Poller.WaitAsync(Timeout, async () =>
            await Sender.Send(new CreateGameVersionCommand(gameId, "1.0.0", null)));
        created.IsSuccess.Should().BeTrue();

        return await UploadAndPublishAsync(gameId, created.Value);
    }

    private async Task<(Guid VersionId, Guid FileId)> PublishVersionAsync(Guid gameId, string version)
    {
        Result<Guid> created = await Sender.Send(new CreateGameVersionCommand(gameId, version, null));
        created.IsSuccess.Should().BeTrue();

        Guid fileId = await UploadAndPublishAsync(gameId, created.Value);
        return (created.Value, fileId);
    }

    private async Task<Guid> UploadAndPublishAsync(Guid gameId, Guid versionId)
    {
        string sha256 = Faker.Random.Hexadecimal(64, string.Empty).ToLowerInvariant();
        Result<RegisterVersionFileResponse> file = await Sender.Send(
            new RegisterVersionFileCommand(versionId, "game.exe", 1024, sha256));
        file.IsSuccess.Should().BeTrue();

        _fileStorage.SimulateUpload($"games/{gameId}/versions/{versionId}/{file.Value.FileId}", 1024, sha256);

        (await Sender.Send(new ConfirmVersionFileUploadCommand(versionId, file.Value.FileId))).IsSuccess.Should().BeTrue();
        (await Sender.Send(new MarkVersionReadyCommand(versionId))).IsSuccess.Should().BeTrue();
        (await Sender.Send(new PublishGameVersionCommand(versionId))).IsSuccess.Should().BeTrue();

        return file.Value.FileId;
    }

    private Task<Result<FileDownloadUrlResponse>> WaitUntilDownloadableAsync(Guid userId, Guid fileId) =>
        Poller.WaitAsync(Timeout, async () => await Sender.Send(new GetFileDownloadUrlQuery(userId, fileId)));

    private Task<Result<bool>> WaitUntilForbiddenAsync(Guid userId, Guid gameId, Guid fileId) =>
        Poller.WaitAsync(Timeout, async () =>
        {
            Result<FileDownloadUrlResponse> download = await Sender.Send(new GetFileDownloadUrlQuery(userId, fileId));

            return download.IsFailure && download.Error == DistributionErrors.GameNotOwned(gameId)
                ? Result.Success(true)
                : Result.Failure<bool>(Error.Failure("Distribution.StillOwned", "Ownership not revoked yet"));
        });
}
