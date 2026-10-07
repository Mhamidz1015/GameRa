using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameRa.Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDistributionPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "users",
                table: "permissions",
                column: "code",
                values: new object[]
                {
                    "distribution:download",
                    "distribution:manage"
                });

            migrationBuilder.InsertData(
                schema: "users",
                table: "role_permissions",
                columns: new[] { "permission_code", "role_name" },
                values: new object[,]
                {
                    { "distribution:download", "Administrator" },
                    { "distribution:download", "Member" },
                    { "distribution:manage", "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "users",
                table: "role_permissions",
                keyColumns: new[] { "permission_code", "role_name" },
                keyValues: new object[] { "distribution:download", "Administrator" });

            migrationBuilder.DeleteData(
                schema: "users",
                table: "role_permissions",
                keyColumns: new[] { "permission_code", "role_name" },
                keyValues: new object[] { "distribution:download", "Member" });

            migrationBuilder.DeleteData(
                schema: "users",
                table: "role_permissions",
                keyColumns: new[] { "permission_code", "role_name" },
                keyValues: new object[] { "distribution:manage", "Administrator" });

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "distribution:download");

            migrationBuilder.DeleteData(
                schema: "users",
                table: "permissions",
                keyColumn: "code",
                keyValue: "distribution:manage");
        }
    }
}
