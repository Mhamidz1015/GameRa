namespace GameRa.Modules.Store.PublicApi;

public interface IStoreApi
{
    Task CreateCustomerAsync(
        Guid customerId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default);
}
