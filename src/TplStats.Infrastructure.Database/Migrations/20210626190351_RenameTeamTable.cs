namespace TplStats.Infrastructure.Database.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// Rename the team table to 'teams'.
    /// </summary>
    public partial class RenameTeamTable : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_team_seasons_season_id",
                table: "team");

            migrationBuilder.DropPrimaryKey(
                name: "pk_team",
                table: "team");

            migrationBuilder.RenameTable(
                name: "team",
                newName: "teams");

            migrationBuilder.RenameIndex(
                name: "ix_team_season_id_name",
                table: "teams",
                newName: "ix_teams_season_id_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_teams",
                table: "teams",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_teams_seasons_season_id",
                table: "teams",
                column: "season_id",
                principalTable: "seasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_teams_seasons_season_id",
                table: "teams");

            migrationBuilder.DropPrimaryKey(
                name: "pk_teams",
                table: "teams");

            migrationBuilder.RenameTable(
                name: "teams",
                newName: "team");

            migrationBuilder.RenameIndex(
                name: "ix_teams_season_id_name",
                table: "team",
                newName: "ix_team_season_id_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_team",
                table: "team",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_team_seasons_season_id",
                table: "team",
                column: "season_id",
                principalTable: "seasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
