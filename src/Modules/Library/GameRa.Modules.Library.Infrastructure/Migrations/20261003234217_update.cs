using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRa.Modules.Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_favorite",
                schema: "libraryitem",
                table: "library_items",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "playtime_records",
                schema: "libraryitem",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_minutes = table.Column<int>(type: "integer", nullable: false),
                    last_played_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_playtime_records", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_playtime_records_user_id_game_id",
                schema: "libraryitem",
                table: "playtime_records",
                columns: new[] { "user_id", "game_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "playtime_records",
                schema: "libraryitem");

            migrationBuilder.DropColumn(
                name: "is_favorite",
                schema: "libraryitem",
                table: "library_items");
        }
    }
}
