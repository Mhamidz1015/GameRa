using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRa.Modules.Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_categories_category_id",
                schema: "games",
                table: "games");

            migrationBuilder.AlterColumn<Guid>(
                name: "category_id",
                schema: "games",
                table: "games",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_games_categories_category_id",
                schema: "games",
                table: "games",
                column: "category_id",
                principalSchema: "games",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_games_categories_category_id",
                schema: "games",
                table: "games");

            migrationBuilder.AlterColumn<Guid>(
                name: "category_id",
                schema: "games",
                table: "games",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_games_categories_category_id",
                schema: "games",
                table: "games",
                column: "category_id",
                principalSchema: "games",
                principalTable: "categories",
                principalColumn: "id");
        }
    }
}
