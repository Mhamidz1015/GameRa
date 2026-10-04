using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameRa.Modules.Store.Infrastructure.Database;

// Delegates every operation to the EF transaction so that EF's Database.CurrentTransaction
// is cleared on commit / rollback / dispose and never leaks a completed transaction
// into later queries made with the same DbContext.
internal sealed class EfCoreDbTransaction(IDbContextTransaction transaction) : DbTransaction
{
    private readonly DbTransaction _inner = transaction.GetDbTransaction();

    protected override DbConnection? DbConnection => _inner.Connection;

    public override IsolationLevel IsolationLevel => _inner.IsolationLevel;

    public override void Commit() => transaction.Commit();

    public override void Rollback() => transaction.Rollback();

    public override Task CommitAsync(CancellationToken cancellationToken = default)
        => transaction.CommitAsync(cancellationToken);

    public override Task RollbackAsync(CancellationToken cancellationToken = default)
        => transaction.RollbackAsync(cancellationToken);

    public override ValueTask DisposeAsync() => transaction.DisposeAsync();

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            transaction.Dispose();
        }
    }
}