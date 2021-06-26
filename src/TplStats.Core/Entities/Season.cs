namespace TplStats.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;

    /// <summary>
    /// A season of competition.
    /// </summary>
    public class Season : IEntity
    {
        private readonly List<Team> teams;
        private Period period;

        /// <summary>
        /// Initializes a new instance of the <see cref="Season"/> class.
        /// </summary>
        /// <param name="id">The season's id.</param>
        /// <param name="name">The season's name.</param>
        /// <param name="startDate">The season's start date.</param>
        /// <param name="endDate">The season's end date.</param>
        public Season(int id, string name, LocalDate startDate, LocalDate endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentOutOfRangeException(nameof(endDate), endDate, "end date must not be earlier than start date");
            }

            Id = id;
            Name = name;
            StartDate = startDate;
            period = endDate - startDate;

            teams = new();
        }

        /// <summary>
        /// Gets the season's id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the season's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the season's start date.
        /// </summary>
        public LocalDate StartDate { get; set; }

        /// <summary>
        /// Gets or sets the season's end date.
        /// </summary>
        public LocalDate EndDate
        {
            get => StartDate + period;
            set
            {
                if (value < StartDate)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "end date must not be earlier than start date");
                }

                period = value - StartDate;
            }
        }

        /// <summary>
        /// Gets the teams competing in the season.
        /// </summary>
        public IReadOnlyCollection<Team> Teams => teams.AsReadOnly();

        /// <summary>
        /// Adds a <see cref="Team"/> to the season.
        /// </summary>
        /// <param name="id">The team's id.</param>
        /// <param name="name">The team's name.</param>
        /// <returns>The team.</returns>
        public Team AddTeam(int id, string name)
        {
            if (Teams.Any(t => t.Name == name))
            {
                throw new ArgumentException("team names must be unique within a season", nameof(name));
            }

            var team = new Team(id, name);
            teams.Add(team);
            return team;
        }
    }
}
