using GameRa.Common.Domain.Abstractions;
using System;

namespace GameRa.Modules.Store.Domain.Customers;

public sealed class Customer : Entity
{
    private Customer()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; }

    public string Username { get; private set; }

    public static Customer Create(Guid id, string email, string username)
    {
        return new Customer
        {
            Id = id,
            Email = email,
            Username = username
        };
    }

    public void Update(string username)
    {
        Username = username;
    }
}
