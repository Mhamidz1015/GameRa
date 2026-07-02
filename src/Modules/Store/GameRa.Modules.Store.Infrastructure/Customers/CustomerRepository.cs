using GameRa.Modules.Store.Domain.Customers;
using GameRa.Modules.Store.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace GameRa.Modules.Store.Infrastructure.Customers;

internal sealed class CustomerRepository(StoreDbContext context) : ICustomerRepository
{
    public async Task<Customer?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Insert(Customer customer)
    {
        context.Customers.Add(customer);
    }
}
