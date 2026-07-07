using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymApp.NTier.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRepositoryAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "DurationInMonths", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Basic Plan", 250.00m },
                    { 2, 3, "Premium Plan", 600.00m },
                    { 3, 12, "VIP Annual", 2000.00m }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "FirstName", "LastName", "Specialization" },
                values: new object[,]
                {
                    { 1, "John", "Doe", "Bodybuilding" },
                    { 2, "Jane", "Smith", "Yoga & Cardio" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Email", "FirstName", "JoinDate", "LastName", "PhoneNumber", "PlanId" },
                values: new object[,]
                {
                    { 1, "ahmet@test.com", "Ahmet", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yilmaz", "555-0001", 2 },
                    { 2, "ayse@test.com", "Ayse", new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Demir", "555-0002", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
