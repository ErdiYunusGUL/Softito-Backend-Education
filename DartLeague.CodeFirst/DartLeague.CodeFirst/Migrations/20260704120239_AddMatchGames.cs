using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DartLeague.CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    GameOrder = table.Column<int>(type: "int", nullable: false),
                    IsDoubles = table.Column<bool>(type: "bit", nullable: false),
                    HomePlayer1Id = table.Column<int>(type: "int", nullable: true),
                    HomePlayer2Id = table.Column<int>(type: "int", nullable: true),
                    AwayPlayer1Id = table.Column<int>(type: "int", nullable: true),
                    AwayPlayer2Id = table.Column<int>(type: "int", nullable: true),
                    HomeScore = table.Column<int>(type: "int", nullable: false),
                    AwayScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchGames_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchGames_Players_AwayPlayer1Id",
                        column: x => x.AwayPlayer1Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchGames_Players_AwayPlayer2Id",
                        column: x => x.AwayPlayer2Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchGames_Players_HomePlayer1Id",
                        column: x => x.HomePlayer1Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchGames_Players_HomePlayer2Id",
                        column: x => x.HomePlayer2Id,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchGames_AwayPlayer1Id",
                table: "MatchGames",
                column: "AwayPlayer1Id");

            migrationBuilder.CreateIndex(
                name: "IX_MatchGames_AwayPlayer2Id",
                table: "MatchGames",
                column: "AwayPlayer2Id");

            migrationBuilder.CreateIndex(
                name: "IX_MatchGames_HomePlayer1Id",
                table: "MatchGames",
                column: "HomePlayer1Id");

            migrationBuilder.CreateIndex(
                name: "IX_MatchGames_HomePlayer2Id",
                table: "MatchGames",
                column: "HomePlayer2Id");

            migrationBuilder.CreateIndex(
                name: "IX_MatchGames_MatchId",
                table: "MatchGames",
                column: "MatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchGames");
        }
    }
}
