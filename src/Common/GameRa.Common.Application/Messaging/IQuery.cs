using GameRa.Common.Domain.Abstractions;
using MediatR;

namespace GameRa.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
