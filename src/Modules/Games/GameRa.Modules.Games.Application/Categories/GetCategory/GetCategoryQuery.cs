using GameRa.Common.Application.Messaging;

namespace GameRa.Modules.Games.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(Guid CategoryId) : IQuery<CategoryResponse>;
