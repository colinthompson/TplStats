namespace TplStats.Core.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;
    using TplStats.Core.Exceptions;

    /// <summary>
    /// A season of competition.
    /// </summary>
    public class Season : IEntity
    {
        private readonly List<Team> teams;
        private readonly List<Game> games;
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
            games = new();
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
        /// Gets the season's schedule.
        /// </summary>
        /// <returns>The games played as part of the season.</returns>
        public IReadOnlyCollection<Game> Games => games.AsReadOnly();

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

        /// <summary>
        /// Schedules a game to be played.
        /// </summary>
        /// <param name="id">The game's id.</param>
        /// <param name="startTime">The time at which the game starts.</param>
        /// <param name="endTime">The time at which the game ends.</param>
        /// <param name="field">The field the game will be played on.</param>
        /// <param name="homeTeam">The home team.</param>
        /// <param name="awayTeam">The away team.</param>
        /// <returns>The newly scheduled game.</returns>
        public Game ScheduleGame(int id, LocalDateTime startTime, LocalDateTime endTime, string field, Team homeTeam, Team awayTeam)
        {
            var game = new Game(id, startTime, endTime, field, homeTeam, awayTeam);

            if (startTime.Date < StartDate || endTime.Date > EndDate)
            {
                throw new ScheduleConflictException(game, "game must occur during the season");
            }

            var conflicts = Games.Where(g => game.ConflictsWith(g));
            if (conflicts.Any())
            {
                throw new ScheduleConflictException(game, "game conflicts with previously scheduled games", conflicts);
            }

            games.Add(game);
            return game;
        }
    }
}
