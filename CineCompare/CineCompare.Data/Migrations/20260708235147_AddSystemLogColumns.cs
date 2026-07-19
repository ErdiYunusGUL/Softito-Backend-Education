using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineCompare.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemLogColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogLevel",
                table: "SystemLogs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ExceptionDetails",
                table: "SystemLogs",
                newName: "LogType");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SystemLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2026, 7, 9, 20, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartTime",
                value: new DateTime(2026, 7, 9, 21, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SystemLogs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SystemLogs",
                newName: "LogLevel");

            migrationBuilder.RenameColumn(
                name: "LogType",
                table: "SystemLogs",
                newName: "ExceptionDetails");

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2026, 7, 8, 20, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartTime",
                value: new DateTime(2026, 7, 8, 21, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
