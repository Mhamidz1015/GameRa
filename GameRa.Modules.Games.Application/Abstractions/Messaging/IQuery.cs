using GameRa.Modules.Games.Domain.Abstractions;
using MediatR;

namespace GameRa.Modules.Games.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
