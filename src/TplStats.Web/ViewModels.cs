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
    }
}
