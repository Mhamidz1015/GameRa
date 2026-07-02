using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Store.Application.Customers.CreateCustomer;

public sealed record CreateCustomerCommand( Guid CustomerId, string Email,string Username)
    : ICommand;
