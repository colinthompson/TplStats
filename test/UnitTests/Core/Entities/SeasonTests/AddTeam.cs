namespace UnitTests.Core.Entities.SeasonTests
{
    using System;
    using System.Linq;
    using TplStats.Core.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="Season.AddTeam(int, string)"/>.
    /// </summary>
    public class AddTeam
    {
        private readonly Season season = new(1, "test", default, default);

        /// <summary>
        /// Ensures a team can be added to the season.
        /// </summary>
        [Fact]
        public void AddsTeamToSeason()
        {
            // Arrange
            const int id = 42;
            const string name = "Test";

            // Act
            var team = season.AddTeam(id, name);

            // Assert
            Assert.Equal(id, team.Id);
            Assert.Equal(name, team.Name);
            Assert.Single(season.Teams.Select(s => s.Id), id);
        }

        /// <summary>
        /// Ensures team names must be unique within a season.
        /// </summary>
        [Fact]
        public void RejectsDuplicateTeamNames()
        {
            // Arrange
            const string name = "Test";
            season.AddTeam(1, name);

            // Act
            var e = Assert.Throws<ArgumentException>(() => season.AddTeam(2, name));

            // Assert
            Assert.Equal("name", e.ParamName);
        }
    }
}
