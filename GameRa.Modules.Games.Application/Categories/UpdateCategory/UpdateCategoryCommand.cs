using GameRa.Modules.Games.Application.Abstractions.Messaging;

namespace GameRa.Modules.Games.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(Guid CategoryId, string Name) : ICommand;
