using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRa.Modules.Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDenormalizedFieldsToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "games",
                table: "games",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "developer",
                schema: "games",
                table: "games",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "games",
                table: "games",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "cover_image_url",
                schema: "games",
                table: "games",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "base_price",
                schema: "games",
                table: "games",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<decimal>(
                name: "active_discount_amount",
                schema: "games",
                table: "games",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "average_rating",
                schema: "games",
                table: "games",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "is_discount_percentage",
                schema: "games",
                table: "games",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_reviews",
                schema: "games",
                table: "games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active_discount_amount",
                schema: "games",
                table: "games");

            migrationBuilder.DropColumn(
                name: "average_rating",
                schema: "games",
                table: "games");

            migrationBuilder.DropColumn(
                name: "is_discount_percentage",
                schema: "games",
                table: "games");

            migrationBuilder.DropColumn(
                name: "total_reviews",
                schema: "games",
                table: "games");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                schema: "games",
                table: "games",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "developer",
                schema: "games",
                table: "games",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                schema: "games",
                table: "games",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "cover_image_url",
                schema: "games",
                table: "games",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<decimal>(
                name: "base_price",
                schema: "games",
                table: "games",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
