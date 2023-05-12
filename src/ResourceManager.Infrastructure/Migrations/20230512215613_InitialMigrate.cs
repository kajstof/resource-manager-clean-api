using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "resources",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_withdrawn = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_resources", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_valid = table.Column<bool>(type: "boolean", nullable: false),
                    valid_to = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => new { x.resource_id, x.id });
                    table.ForeignKey(
                        name: "fk_items_resources_resource_id",
                        column: x => x.resource_id,
                        principalSchema: "public",
                        principalTable: "resources",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_items_id",
                schema: "public",
                table: "items",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_resources_id",
                schema: "public",
                table: "resources",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "items",
                schema: "public");

            migrationBuilder.DropTable(
                name: "resources",
                schema: "public");
        }
    }
}
