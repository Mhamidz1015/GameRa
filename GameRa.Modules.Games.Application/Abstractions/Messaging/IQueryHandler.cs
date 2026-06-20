using GameRa.Modules.Games.Domain.Abstractions;
using MediatR;

namespace GameRa.Modules.Games.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
