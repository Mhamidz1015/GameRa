using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Games.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : ICommand<Guid>;
