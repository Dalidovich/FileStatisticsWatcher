using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStatisticsWatcher.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "file_settings",
                columns: table => new
                {
                    pk_file_settings_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    path = table.Column<string>(type: "character varying", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    depth = table.Column<short>(type: "smallint", nullable: false),
                    extension = table.Column<string>(type: "character varying", nullable: false),
                    create_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_write_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    last_access_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_settings", x => x.pk_file_settings_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_file_settings_pk_file_settings_id",
                table: "file_settings",
                column: "pk_file_settings_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file_settings");
        }
    }
}
