using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRa.Modules.Reviews.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reviews");

            migrationBuilder.CreateTable(
                name: "inbox_message_consumers",
                schema: "reviews",
                columns: table => new
                {
                    inbox_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_message_consumers", x => new { x.inbox_message_id, x.name });
                });

            migrationBuilder.CreateTable(
                name: "inbox_messages",
                schema: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "jsonb", maxLength: 2000, nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message_consumers",
                schema: "reviews",
                columns: table => new
                {
                    outbox_message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_message_consumers", x => new { x.outbox_message_id, x.name });
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "jsonb", maxLength: 2000, nullable: false),
                    occurred_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                schema: "reviews",
                columns: table => new
                {
                    review_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_verified_purchase = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reviews", x => x.review_id);
                });

            migrationBuilder.CreateTable(
                name: "verified_purchases",
                schema: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    purchased_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_verified_purchases", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reviews_game_id_user_id",
                schema: "reviews",
                table: "reviews",
                columns: new[] { "game_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_verified_purchases_game_id_user_id",
                schema: "reviews",
                table: "verified_purchases",
                columns: new[] { "game_id", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_message_consumers",
                schema: "reviews");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "reviews");

            migrationBuilder.DropTable(
                name: "outbox_message_consumers",
                schema: "reviews");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "reviews");

            migrationBuilder.DropTable(
                name: "reviews",
                schema: "reviews");

            migrationBuilder.DropTable(
                name: "verified_purchases",
                schema: "reviews");
        }
    }
}
