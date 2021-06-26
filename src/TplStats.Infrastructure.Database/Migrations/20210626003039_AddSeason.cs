namespace TplStats.Infrastructure.Database.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using NodaTime;

    /// <summary>
    /// Add the season entity.
    /// </summary>
    public partial class AddSeason : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "seasons",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<LocalDate>(type: "date", nullable: false),
                    end_date = table.Column<LocalDate>(type: "date", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_seasons", x => x.id);
                    table.CheckConstraint("end_date_not_before_start_date", "start_date <= end_date");
                });

            migrationBuilder.CreateIndex(
                name: "ix_seasons_name",
                table: "seasons",
                column: "name",
                unique: true);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "seasons");
        }
    }
}
