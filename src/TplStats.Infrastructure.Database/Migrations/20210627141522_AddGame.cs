namespace TplStats.Infrastructure.Database.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using NodaTime;

    /// <summary>
    /// Adds game entity to the database.
    /// </summary>
    public partial class AddGame : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    start_time = table.Column<LocalDateTime>(type: "timestamp", nullable: false),
                    end_time = table.Column<LocalDateTime>(type: "timestamp", nullable: false),
                    field = table.Column<string>(type: "text", nullable: false),
                    home_team_id = table.Column<int>(type: "integer", nullable: false),
                    away_team_id = table.Column<int>(type: "integer", nullable: false),
                    season_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_games", x => x.id);
                    table.CheckConstraint("end_time_not_before_start_time", "start_time <= end_time");
                    table.ForeignKey(
                        name: "fk_games_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_games_teams_team_id",
                        column: x => x.away_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_games_teams_team_id1",
                        column: x => x.home_team_id,
                        principalTable: "teams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_games_away_team_id",
                table: "games",
                column: "away_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_home_team_id",
                table: "games",
                column: "home_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_games_season_id",
                table: "games",
                column: "season_id");
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "games");
        }
    }
}
