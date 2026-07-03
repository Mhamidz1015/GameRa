using GameRa.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameRa.Modules.Users.Infrastructure.Users;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);

        builder.HasData(
            Permission.GetUser,
            Permission.ModifyUser,
            Permission.GetGames,
            Permission.SearchGames,
            Permission.ModifyGames,
            Permission.GetCategories,
            Permission.ModifyCategories,
            Permission.GetCart,
            Permission.AddToCart,
            Permission.RemoveFromCart,
            Permission.GetOrders,
            Permission.CreateOrder);

        builder
            .HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");

                joinBuilder.HasData(
                    // Member permissions
                    CreateRolePermission(Role.Member, Permission.GetUser),
                    CreateRolePermission(Role.Member, Permission.ModifyUser),
                    CreateRolePermission(Role.Member, Permission.SearchGames),
                    CreateRolePermission(Role.Member, Permission.GetCart),
                    CreateRolePermission(Role.Member, Permission.AddToCart),
                    CreateRolePermission(Role.Member, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Member, Permission.GetOrders),
                    CreateRolePermission(Role.Member, Permission.CreateOrder),
                    // Admin permissions
                    CreateRolePermission(Role.Administrator, Permission.GetUser),
                    CreateRolePermission(Role.Administrator, Permission.ModifyUser),
                    CreateRolePermission(Role.Administrator, Permission.GetGames),
                    CreateRolePermission(Role.Administrator, Permission.SearchGames),
                    CreateRolePermission(Role.Administrator, Permission.ModifyGames),
                    CreateRolePermission(Role.Administrator, Permission.GetCategories),
                    CreateRolePermission(Role.Administrator, Permission.ModifyCategories),
                    CreateRolePermission(Role.Administrator, Permission.GetCart),
                    CreateRolePermission(Role.Administrator, Permission.AddToCart),
                    CreateRolePermission(Role.Administrator, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Administrator, Permission.GetOrders),
                    CreateRolePermission(Role.Administrator, Permission.CreateOrder));
            });
    }

    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Code
        };
    }
}
