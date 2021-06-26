namespace TplStats.Web
{
    using AutoMapper;
    using TplStats.Core.Entities;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// AutoMapper profile.
    /// </summary>
    internal class TplStatsMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TplStatsMapperProfile"/> class.
        /// </summary>
        public TplStatsMapperProfile()
        {
            CreateMap<Season, SeasonModel>();
            CreateMap<Team, TeamModel>();
        }
    }
}
