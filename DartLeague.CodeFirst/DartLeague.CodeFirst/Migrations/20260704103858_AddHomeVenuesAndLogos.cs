using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DartLeague.CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeVenuesAndLogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Venues",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HomeVenueId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Teams",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_HomeVenueId",
                table: "Teams",
                column: "HomeVenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Venues_HomeVenueId",
                table: "Teams",
                column: "HomeVenueId",
                principalTable: "Venues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Venues_HomeVenueId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_HomeVenueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "HomeVenueId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Teams");
        }
    }
}
