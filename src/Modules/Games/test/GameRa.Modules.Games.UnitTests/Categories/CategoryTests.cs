using FluentAssertions;
using GameRa.Common.Domain.Abstractions;
using GameRa.Modules.Games.Domain.Categories;
using GameRa.Modules.Games.UnitTests.Abstractions;

namespace GameRa.Modules.Games.UnitTests.Categories;

public class CategoryTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnFailure_WhenNameIsEmpty()
    {
        // Act
        Result<Category> result = Category.Create(string.Empty);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.NameIsEmpty);
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenCategoryIsCreated()
    {
        //Act
        Result<Category> result = Category.Create(Faker.Music.Genre());

        //Assert
        CategoryCreatedDomainEvent domainEvent =
            AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(result.Value);

        domainEvent.CategoryId.Should().Be(result.Value.Id);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenCategoryIsArchived()
    {
        //Arrange
        Result<Category> result = Category.Create(Faker.Music.Genre());

        Category category = result.Value;

        //Act
        category.Archive();

        //Assert
        CategoryArchivedDomainEvent domainEvent =
            AssertDomainEventWasPublished<CategoryArchivedDomainEvent>(category);

        domainEvent.CategoryId.Should().Be(category.Id);
    }

    [Fact]
    public void ChangeName_ShouldRaiseDomainEvent_WhenCategoryNameIsChanged()
    {
        //Arrange
        Result<Category> result = Category.Create(Faker.Music.Genre());
        Category category = result.Value;
        category.ClearDomainEvents();

        string newName = Faker.Music.Genre();

        //Act
        category.ChangeName(newName);
        
        //Assert
        CategoryNameChangedDomainEvent domainEvent =
            AssertDomainEventWasPublished<CategoryNameChangedDomainEvent>(category);

        domainEvent.CategoryId.Should().Be(category.Id);
    }
}
