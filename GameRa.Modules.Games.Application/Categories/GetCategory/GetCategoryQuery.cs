using GameRa.Modules.Games.Application.Abstractions.Messaging;

namespace GameRa.Modules.Games.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryResponse>;
