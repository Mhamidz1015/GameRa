using GameRa.Common.Application.Data;
using GameRa.Common.Infrastructure.Interceptors;
using GameRa.Common.Presentation.Endpoints;
using GameRa.Modules.Store.Application.Abstractions.Payments;
using GameRa.Modules.Store.Application.Carts;
using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.Domain.Games;
using GameRa.Modules.Store.Domain.Orders;
using GameRa.Modules.Store.Domain.Payments;
using GameRa.Modules.Store.Infrastructure.Customers;
using GameRa.Modules.Store.Infrastructure.Database;
using GameRa.Modules.Store.Infrastructure.Games;
using GameRa.Modules.Store.Infrastructure.Orders;
using GameRa.Modules.Store.Infrastructure.Payments;
using GameRa.Modules.Store.Presentation.Customers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameRa.Modules.Store.Infrastructure;

public static class StoreModule
{
    public static IServiceCollection AddStoreModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }
     
    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StoreDbContext>((sp, options) =>
            options
                .UseNpgsql(
                    configuration.GetConnectionString("Database"),
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Store))
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>())
                .UseSnakeCaseNamingConvention());

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IGameRepository,GameRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<StoreDbContext>());

        services.AddSingleton<CartService>();
        services.AddSingleton<IPaymentService, PaymentService>();
    }
}
