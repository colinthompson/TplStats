namespace TplStats.Core.Entities
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A team that competes in a <see cref="Season"/>.
    /// </summary>
    public class Team : IEntity
    {
        private readonly List<Game> homeGames;
        private readonly List<Game> awayGames;

        /// <summary>
        /// Initializes a new instance of the <see cref="Team"/> class.
        /// </summary>
        /// <param name="id">The team's id.</param>
        /// <param name="name">The team's name.</param>
        internal Team(int id, string name)
        {
            Id = id;
            Name = name;

            homeGames = new();
            awayGames = new();
        }

        /// <summary>
        /// Gets the team's id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the team's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the games in which the team is competing as the home team.
        /// </summary>
        public IReadOnlyCollection<Game> HomeGames => homeGames.AsReadOnly();

        /// <summary>
        /// Gets the games in which the team is competing as the away team.
        /// </summary>
        public IReadOnlyCollection<Game> AwayGames => awayGames.AsReadOnly();

        /// <summary>
        /// Gets the games in which the team is competing, regardless of role.
        /// </summary>
        public IReadOnlyCollection<Game> Games => HomeGames.Union(AwayGames).ToList().AsReadOnly();
    }
}
