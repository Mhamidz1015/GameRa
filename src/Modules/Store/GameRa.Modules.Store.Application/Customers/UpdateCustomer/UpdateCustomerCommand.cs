using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(Guid CustomerId, string Username) : ICommand;
