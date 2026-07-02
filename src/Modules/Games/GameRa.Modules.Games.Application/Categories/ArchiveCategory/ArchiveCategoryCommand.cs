using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Games.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(Guid CategoryId) : ICommand;
