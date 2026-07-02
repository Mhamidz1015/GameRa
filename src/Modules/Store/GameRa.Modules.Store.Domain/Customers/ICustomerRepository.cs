namespace GameRa.Modules.Store.Domain.Customers;

public interface ICustomerRepository
{
    Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Customer customer);
}
