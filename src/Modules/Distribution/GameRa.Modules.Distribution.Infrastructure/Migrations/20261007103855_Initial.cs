using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameRa.Modules.Distribution.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "distribution");

            migrationBuilder.CreateTable(
                name: "distribution_games",
                schema: "distribution",
                columns: table => new
                {
                    game_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_distribution_games", x => x.game_id);
                });

            migrationBuilder.CreateTable(
                name: "game_ownerships",
                schema: "distribution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_owned = table.Column<bool>(type: "boolean", nullable: false),
                    last_event_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_ownerships", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_versions",
                schema: "distribution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_id = table.Column<Guid>(type: "uuid", nullable: false),
                    major = table.Column<int>(type: "integer", nullable: false),
                    minor = table.Column<int>(type: "integer", nullable: false),
                    patch = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    release_notes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    published_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deprecated_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_game_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inbox_message_consumers",
                schema: "distribution",
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
                schema: "distribution",
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
                schema: "distribution",
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
                schema: "distribution",
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
                name: "version_files",
                schema: "distribution",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_version_id = table.Column<Guid>(type: "uuid", nullable: false),
                    relative_path = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    normalized_path = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    size_bytes = table.Column<long>(type: "bigint", nullable: false),
                    sha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    storage_key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    upload_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_version_files", x => x.id);
                    table.ForeignKey(
                        name: "fk_version_files_game_versions_game_version_id",
                        column: x => x.game_version_id,
                        principalSchema: "distribution",
                        principalTable: "game_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_game_ownerships_user_id_game_id",
                schema: "distribution",
                table: "game_ownerships",
                columns: new[] { "user_id", "game_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_versions_game_id_major_minor_patch",
                schema: "distribution",
                table: "game_versions",
                columns: new[] { "game_id", "major", "minor", "patch" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_game_versions_game_id_status",
                schema: "distribution",
                table: "game_versions",
                columns: new[] { "game_id", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_version_files_game_version_id_normalized_path",
                schema: "distribution",
                table: "version_files",
                columns: new[] { "game_version_id", "normalized_path" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "distribution_games",
                schema: "distribution");

            migrationBuilder.DropTable(
                name: "game_ownerships",
                schema: "distribution");

            migrationBuilder.DropTable(
                name: "inbox_message_consumers",
                schema: "distribution");

            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "distribution");

            migrationBuilder.DropTable(
                name: "outbox_message_consumers",
                schema: "distribution");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "distribution");

            migrationBuilder.DropTable(
                name: "version_files",
                schema: "distribution");

            migrationBuilder.DropTable(
                name: "game_versions",
                schema: "distribution");
        }
    }
}
