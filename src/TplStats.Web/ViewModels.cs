namespace TplStats.Web
{
    using NodaTime;

    /// <summary>
    /// Wrapper class for view models.
    /// </summary>
    public static class ViewModels
    {
        /// <summary>
        /// View model for <see cref="TplStats.Core.Entities.Season"/>.
        /// </summary>
        /// <param name="Id">The season's id.</param>
        /// <param name="Name">The season's name.</param>
        /// <param name="StartDate">The season's start date.</param>
        /// <param name="EndDate">The season's end date.</param>
        public record SeasonModel(int Id, string Name, LocalDate StartDate, LocalDate EndDate);

        /// <summary>
        /// View model for <see cref="TplStats.Core.Entities.Team"/>.
        /// </summary>
        /// <param name="Id">The team's id.</param>
        /// <param name="Name">The team's name.</param>
        public record TeamModel(int Id, string Name);

        /// <summary>
        /// View model for <see cref="TplStats.Core.Entities.Game"/>.
        /// </summary>
        /// <param name="Id">The game's id.</param>
        /// <param name="Field">The field on which the game is played.</param>
        /// <param name="StartTime">The time the game starts.</param>
        /// <param name="EndTime">The time the game ends.</param>
        /// <param name="HomeTeamId">The home team's id.</param>
        /// <param name="AwayTeamId">The away team's id.</param>
        public record GameModel(int Id, string Field, LocalDateTime StartTime, LocalDateTime EndTime, int HomeTeamId, int AwayTeamId);
    }
}
