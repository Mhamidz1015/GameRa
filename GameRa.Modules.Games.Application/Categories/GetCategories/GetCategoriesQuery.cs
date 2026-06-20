using GameRa.Modules.Games.Application.Abstractions.Messaging;
using GameRa.Modules.Games.Application.Categories.GetCategory;

namespace GameRa.Modules.Games.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyCollection<CategoryResponse>>;
