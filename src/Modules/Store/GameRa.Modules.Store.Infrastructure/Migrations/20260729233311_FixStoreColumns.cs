using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRa.Modules.Store.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                schema: "store",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "currency",
                schema: "store",
                table: "games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                schema: "store",
                table: "order_items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                schema: "store",
                table: "games",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
