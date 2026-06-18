using System.Data.Common;
using GameRa.Modules.Games.Application.Abstractions.Data;
using Npgsql;

namespace GameRa.Modules.Games.Infrastructure.Data;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
