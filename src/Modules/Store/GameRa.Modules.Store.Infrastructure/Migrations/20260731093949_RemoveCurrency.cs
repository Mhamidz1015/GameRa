using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRa.Modules.Store.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currency",
                schema: "store",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "currency",
                schema: "store",
                table: "orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "currency",
                schema: "store",
                table: "payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                schema: "store",
                table: "orders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
