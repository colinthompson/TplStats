namespace UnitTests.Core.Entities.SeasonTests
{
    using System.Linq;
    using NodaTime;
    using TplStats.Core.Entities;
    using TplStats.Core.Exceptions;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="Season.ScheduleGame(int, LocalDateTime, LocalDateTime, string, Team, Team)"/>.
    /// </summary>
    public class ScheduleGame
    {
        private readonly Season season = new(1, "Test", new LocalDate(2021, 01, 01), new LocalDate(2021, 12, 31));
        private readonly Team[] teams;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleGame"/> class.
        /// </summary>
        public ScheduleGame()
        {
            teams = Enumerable.Range(1, 5)
                .Select(x => season.AddTeam(x, $"Team #{x}"))
                .ToArray();
        }

        /// <summary>
        /// Ensures games can be scheduled.
        /// </summary>
        /// <param name="firstStartHour">Hour the first game starts at.</param>
        /// <param name="firstStartMinute">Minute the first game starts at.</param>
        /// <param name="secondStartHour">Hour the second game starts at.</param>
        /// <param name="secondStartMinute">Minute the second game starts at.</param>
        [Theory]
        [InlineData(11, 0, 12, 0)] // Ends as the next starts
        [InlineData(12, 0, 11, 0)] // Starts as the first ends
        public void CanScheduleNonConflictingGames(int firstStartHour, int firstStartMinute, int secondStartHour, int secondStartMinute)
        {
            // Arrange
            const int year = 2021;
            const int month = 1;
            const int day = 1;
            const string field = "Field 1";

            var firstStart = new LocalDateTime(year, month, day, firstStartHour, firstStartMinute);
            var firstEnd = firstStart + Period.FromHours(1);

            var secondStart = new LocalDateTime(year, month, day, secondStartHour, secondStartMinute);
            var secondEnd = secondStart + Period.FromHours(1);

            // Act
            var firstGame = season.ScheduleGame(1, firstStart, firstEnd, field, teams[0], teams[1]);
            var secondGame = season.ScheduleGame(2, secondStart, secondEnd, field, teams[0], teams[1]);

            // Assert
            Assert.NotNull(firstGame);
            Assert.NotNull(secondGame);
        }

        /// <summary>
        /// Ensures a field cannot be double booked.
        /// </summary>
        /// <param name="firstStartHour">Hour the first game starts at.</param>
        /// <param name="firstStartMinute">Minute the first game starts at.</param>
        /// <param name="secondStartHour">Hour the second game starts at.</param>
        /// <param name="secondStartMinute">Minute the second game starts at.</param>
        [Theory]
        [InlineData(12, 0, 11, 1)] // Doesn't quite end in time
        [InlineData(12, 0, 12, 0)] // Identical
        [InlineData(12, 0, 12, 59)] // Starts just before
        public void CannotScheduleOverlappingGamesOnTheSameField(int firstStartHour, int firstStartMinute, int secondStartHour, int secondStartMinute)
        {
            // Arrange
            const int year = 2021;
            const int month = 1;
            const int day = 1;
            const string field = "Field 1";

            var firstStart = new LocalDateTime(year, month, day, firstStartHour, firstStartMinute);
            var firstEnd = firstStart + Period.FromHours(1);

            var secondStart = new LocalDateTime(year, month, day, secondStartHour, secondStartMinute);
            var secondEnd = secondStart + Period.FromHours(1);

            // Act
            var firstGame = season.ScheduleGame(1, firstStart, firstEnd, field, teams[0], teams[1]);
            var e = Assert.Throws<ScheduleConflictException>(() => season.ScheduleGame(2, secondStart, secondEnd, field, teams[2], teams[3]));

            // Assert
            Assert.Single(e.Conflicts, firstGame);
        }

        /// <summary>
        /// Ensurse a team cannot be scheduled to play two or more games at the same time.
        /// </summary>
        [Fact]
        public void CannotScheduleSimultaneousGamesWithTheSameTeam()
        {
            // Arrange
            LocalDateTime start = new(2021, 1, 1, 12, 0, 0);
            var end = start + Period.FromHours(1);

            // Act
            var firstGame = season.ScheduleGame(1, start, end, "Field 1", teams[0], teams[1]);
            var e = Assert.Throws<ScheduleConflictException>(() => season.ScheduleGame(2, start, end, "Field 2", teams[0], teams[2]));

            // Assert
            Assert.Single(e.Conflicts, firstGame);
        }

        /// <summary>
        /// Ensures that games are scheduled entirely within the season.
        /// </summary>
        [Fact]
        public void GamesMustBeScheduledDuringTheSeason()
        {
            // Arrange
            const string field = "Field 1";
            var start = season.StartDate.AtMidnight() - Period.FromSeconds(1);
            var end = start + Period.FromHours(1);

            // Act
            var e = Assert.Throws<ScheduleConflictException>(() => season.ScheduleGame(1, start, end, field, teams[0], teams[1]));

            // Assert
            Assert.Empty(e.Conflicts); // It conflicts with the season itself.
        }
    }
}
