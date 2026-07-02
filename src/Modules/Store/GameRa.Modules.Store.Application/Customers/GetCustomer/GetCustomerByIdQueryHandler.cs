using System.Data.Common;
using Dapper;
using GameRa.Common.Application.Data;
using GameRa.Common.Application.Messaging;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Store.Domain.Customers;

namespace GameRa.Modules.Store.Application.Customers.GetCustomer;

internal sealed class GetCustomerByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCustomerQuery, CustomerResponse>
{
    public async Task<Result<CustomerResponse>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(CustomerResponse.Id)},
                 email AS {nameof(CustomerResponse.Email)},
                 first_name AS {nameof(CustomerResponse.Username)},
             FROM Store.customers
             WHERE id = @CustomerId
             """;

        CustomerResponse? customer = await connection.QuerySingleOrDefaultAsync<CustomerResponse>(sql, request);

        if (customer is null)
        {
            return Result.Failure<CustomerResponse>(CustomerErrors.NotFound(request.CustomerId));
        }

        return customer;
    }
}
