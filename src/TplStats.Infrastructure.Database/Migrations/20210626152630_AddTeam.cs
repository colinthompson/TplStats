namespace TplStats.Infrastructure.Database.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using TplStats.Core.Entities;

    /// <summary>
    /// Add <see cref="Team"/> entity to the database.
    /// </summary>
    public partial class AddTeam : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    season_id = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_team", x => x.id);
                    table.ForeignKey(
                        name: "fk_team_seasons_season_id",
                        column: x => x.season_id,
                        principalTable: "seasons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_team_season_id_name",
                table: "team",
                columns: new[] { "season_id", "name" },
                unique: true);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "team");
        }
    }
}
