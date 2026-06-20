using GameRa.Modules.Games.Domain.Abstractions;
using MediatR;

namespace GameRa.Modules.Games.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
