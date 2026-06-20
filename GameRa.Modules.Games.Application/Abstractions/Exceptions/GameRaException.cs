using GameRa.Modules.Games.Domain.Abstractions;

namespace GameRa.Modules.Games.Application.Abstractions.Exceptions;

public sealed class GameRaException : Exception
{
    public GameRaException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
