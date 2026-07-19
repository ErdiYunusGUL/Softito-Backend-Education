using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CineCompare.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Directors",
                columns: new[] { "Id", "Biography", "CreatedDate", "FirstName", "IsActive", "LastName", "ProfileImageUrl" },
                values: new object[,]
                {
                    { 1, "Zamanın efendisi.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christopher", true, "Nolan", "nolan.jpg" },
                    { 2, "Bilimkurgu vizyoneri.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Denis", true, "Villeneuve", "denis.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Theaters",
                columns: new[] { "Id", "Address", "City", "CreatedDate", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Kadıköy Meydan", "İstanbul", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Kadıköy Cineverse" },
                    { 2, "Beşiktaş", "İstanbul", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Zorlu Center Cinens" }
                });

            migrationBuilder.InsertData(
                table: "Halls",
                columns: new[] { "Id", "Capacity", "CreatedDate", "HardwareType", "IsActive", "Name", "TheaterId" },
                values: new object[,]
                {
                    { 1, 150, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IMAX", true, "Salon 1", 1 },
                    { 2, 40, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "VIP 2D", true, "VIP Salon", 2 }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "CreatedDate", "DirectorId", "DurationInMinutes", "Genre", "IsActive", "PosterUrl", "ReleaseDate", "Title", "TrailerUrl" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 169, "Sci-Fi", true, "interstellar.jpg", new DateTime(2014, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Interstellar", "url" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 166, "Sci-Fi", true, "dune2.jpg", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dune: Part Two", "url" }
                });

            migrationBuilder.InsertData(
                table: "Showtimes",
                columns: new[] { "Id", "CreatedDate", "HallId", "IsActive", "LanguageOption", "MovieId", "ScreeningFormat", "StartTime" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, "Altyazılı", 1, "IMAX", new DateTime(2026, 7, 8, 20, 0, 0, 0, DateTimeKind.Local) },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, true, "Altyazılı", 2, "2D", new DateTime(2026, 7, 8, 21, 0, 0, 0, DateTimeKind.Local) }
                });

            migrationBuilder.InsertData(
                table: "TicketPrices",
                columns: new[] { "Id", "CreatedDate", "IsActive", "Price", "ShowtimeId", "TicketCategory" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 200m, 1, "Student" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 250m, 1, "Adult" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 300m, 2, "Student" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 350m, 2, "Adult" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TicketPrices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TicketPrices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TicketPrices",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TicketPrices",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Showtimes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Halls",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Directors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Theaters",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
