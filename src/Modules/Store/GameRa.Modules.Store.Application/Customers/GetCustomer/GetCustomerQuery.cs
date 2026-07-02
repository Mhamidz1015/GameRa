using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Customers.GetCustomer;

public sealed record GetCustomerQuery(Guid CustomerId) : IQuery<CustomerResponse>;
