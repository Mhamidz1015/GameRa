using Amazon.Runtime;
using Amazon.S3;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Application.MessagingEventBus;
using GameRa.Common.Infrastructure.Outbox;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Modules.Distribution.Application;
using GameRa.Modules.Distribution.Application.Abstractions.Data;
using GameRa.Modules.Distribution.Application.Abstractions.Ownership;
using GameRa.Modules.Distribution.Application.Abstractions.Storage;
using GameRa.Modules.Distribution.Domain.Games;
using GameRa.Modules.Distribution.Domain.GameVersions;
using GameRa.Modules.Distribution.Domain.Ownerships;
using GameRa.Modules.Distribution.Infrastructure.Database;
using GameRa.Modules.Distribution.Infrastructure.Games;
using GameRa.Modules.Distribution.Infrastructure.GameVersions;
using GameRa.Modules.Distribution.Infrastructure.Inbox;
using GameRa.Modules.Distribution.Infrastructure.Outbox;
using GameRa.Modules.Distribution.Infrastructure.Ownerships;
using GameRa.Modules.Distribution.Infrastructure.Storage;
using GameRa.Modules.Games.IntegrationEvents;
using GameRa.Modules.Library.IntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace GameRa.Modules.Distribution.Infrastructure;

public static class DistributionModule
{
    public static IServiceCollection AddDistributionModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDomainEventHandlers();

        services.AddIntegrationEventHandlers();

        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<GameAddedIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<LibraryItemAddedIntegrationEvent>>();
        registrationConfigurator.AddConsumer<IntegrationEventConsumer<LibraryItemRemovedIntegrationEvent>>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DistributionDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Distribution))
                .UseSnakeCaseNamingConvention()
                .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<DistributionDbContext>());

        services.AddScoped<IGameVersionRepository, GameVersionRepository>();

        services.AddScoped<IGameOwnershipRepository, GameOwnershipRepository>();

        services.AddScoped<IDistributionGameRepository, DistributionGameRepository>();

        services.AddScoped<IGameOwnershipReader, DapperGameOwnershipReader>();

        services.Configure<DistributionOptions>(configuration.GetSection("Distribution:Options"));

        services.Configure<StorageOptions>(configuration.GetSection("Distribution:Storage"));

        services.TryAddSingleton<IAmazonS3>(sp =>
        {
            StorageOptions storageOptions = sp.GetRequiredService<IOptions<StorageOptions>>().Value;

            return new AmazonS3Client(
                new BasicAWSCredentials(storageOptions.AccessKey, storageOptions.SecretKey),
                new AmazonS3Config
                {
                    ServiceURL = storageOptions.ServiceUrl,
                    AuthenticationRegion = storageOptions.Region,
                    ForcePathStyle = storageOptions.ForcePathStyle
                });
        });

        services.AddSingleton<IFileStorage, S3FileStorage>();

        services.Configure<OutboxOptions>(configuration.GetSection("Distribution:Outbox"));

        services.ConfigureOptions<ConfigureProcessOutboxJob>();

        services.Configure<InboxOptions>(configuration.GetSection("Distribution:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
    }

    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }

    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {
        Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }
}
