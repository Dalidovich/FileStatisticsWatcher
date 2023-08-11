using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStatisticsWatcher.Migrations
{
    /// <inheritdoc />
    public partial class remove_unnecessary_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_write_date",
                table: "file_settings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "last_write_date",
                table: "file_settings",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
