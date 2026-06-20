using GameRa.Modules.Games.Application.Abstractions.Messaging;

namespace GameRa.Modules.Games.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
