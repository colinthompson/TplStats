namespace TplStats.Core.Entities
{
    using System;
    using System.Linq;
    using NodaTime;

    /// <summary>
    /// A game between two teams, played as part of a season.
    /// </summary>
    public class Game : IEntity
    {
        private Period period;
        private Team awayTeam;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="id">The game's id.</param>
        /// <param name="startTime">The time at which the game starts.</param>
        /// <param name="endTime">The time at which the game ends.</param>
        /// <param name="field">The field the game will be played on.</param>
        /// <param name="homeTeam">The home team.</param>
        /// <param name="awayTeam">The away team.</param>
        internal Game(int id, LocalDateTime startTime, LocalDateTime endTime, string field, Team homeTeam, Team awayTeam)
            : this(id, startTime, endTime, field)
        {
            if (homeTeam.Id == awayTeam.Id)
            {
                throw new ArgumentException("a team cannot play against itself", nameof(awayTeam));
            }

            HomeTeam = homeTeam;
            this.awayTeam = awayTeam;
        }

        // Exists because EF Core can't set navigation properties using a constructor
        private Game(int id, LocalDateTime startTime, LocalDateTime endTime, string field)
        {
            if (endTime < startTime)
            {
                throw new ArgumentOutOfRangeException(nameof(endTime), endTime, "end time must not be before start time");
            }

            Id = id;
            StartTime = startTime;
            period = endTime - startTime;
            Field = field;

            HomeTeam = null!; // Set in constructor or by EF Core
            awayTeam = null!; // Set in constructor or by EF Core
        }

        /// <summary>
        /// Gets the game's id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the game's start time.
        /// </summary>
        public LocalDateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the game's end time.
        /// </summary>
        public LocalDateTime EndTime
        {
            get => StartTime + period;
            set => period = value - StartTime;
        }

        /// <summary>
        /// Gets or sets the game's field.
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the game's home team.
        /// </summary>
        public Team HomeTeam { get; set; }

        /// <summary>
        /// Gets or sets the game's away team.
        /// </summary>
        public Team AwayTeam
        {
            get => awayTeam;
            set
            {
                if (value.Id == HomeTeam.Id)
                {
                    throw new ArgumentException("a team cannot play against itself", nameof(value));
                }

                awayTeam = value;
            }
        }

        /// <summary>
        /// Determines whether or not a given game conflicts with this one.
        /// </summary>
        /// <remarks>
        /// A conflict between two games is defined as a situation in which the same field is scheduled for use by two or more games at the same time, or when the same team is scheduled to play two or more games at the same time.
        /// </remarks>
        /// <param name="game">The other game.</param>
        /// <returns><c>true</c> if the games conflict; <c>false</c> otherwise.</returns>
        internal bool ConflictsWith(Game game)
        {
            var teams = new[] { HomeTeam, AwayTeam };
            var otherTeams = new[] { game.HomeTeam, game.AwayTeam };

            // First, determine if the two games overlap in time. If they don't, there's no conflict
            if (StartTime < game.EndTime && EndTime > game.StartTime)
            {
                if (Field.Equals(game.Field))
                {
                    return true;
                }
                else if (teams.Intersect(otherTeams).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
