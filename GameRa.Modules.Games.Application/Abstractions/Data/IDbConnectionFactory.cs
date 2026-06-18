using System.Data.Common;

namespace GameRa.Modules.Games.Application.Abstractions.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}
