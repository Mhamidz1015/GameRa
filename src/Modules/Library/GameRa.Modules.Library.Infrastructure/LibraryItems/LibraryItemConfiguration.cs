using GameRa.Modules.Library.Domain.LibraryItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Library.Infrastructure.LibraryItems;

internal sealed class LibraryItemConfiguration : IEntityTypeConfiguration<LibraryItem>
{
    public void Configure(EntityTypeBuilder<LibraryItem> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.GametitleSnapshot); 

        builder.Property(c => c.UserId).IsRequired();

        builder.Property(c => c.GameId).IsRequired();

        builder.Property(x => x.IsArchived)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.UserId,
            x.GameId
        }).IsUnique();
    }
}
