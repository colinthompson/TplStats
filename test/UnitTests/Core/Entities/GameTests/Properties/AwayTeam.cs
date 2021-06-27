namespace UnitTests.Core.Entities.GameTests.Properties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NodaTime;
    using TplStats.Core.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="Game.AwayTeam"/>.
    /// </summary>
    public class AwayTeam
    {
        private readonly Season season = new(1, "test", new LocalDate(2021, 01, 01), new LocalDate(2021, 6, 1));
        private readonly List<Team> teams;
        private readonly Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwayTeam"/> class.
        /// </summary>
        public AwayTeam()
        {
            LocalDateTime startTime = season.StartDate.AtMidnight();
            LocalDateTime endTime = startTime + Period.FromHours(1);

            teams = Enumerable.Range(1, 5)
                .Select(x => season.AddTeam(x, $"Team #{x}"))
                .ToList();
            game = season.ScheduleGame(1, startTime, endTime, "Test Field", teams[0], teams[1]);
        }

        /// <summary>
        /// Ensures that a team cannot be scheduled to play against itself.
        /// </summary>
        [Fact]
        public void MustNotBeSameTeamAsHomeTeam()
        {
            // Arrange

            // Act
            var e = Assert.Throws<ArgumentException>(() => game.AwayTeam = game.HomeTeam);

            // Assert
            Assert.Equal("value", e.ParamName);
        }
    }
}
