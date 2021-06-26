namespace UnitTests.Core.Entities.SeasonTests.Properties
{
    using System;
    using NodaTime;
    using TplStats.Core.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="Season.EndDate"/>.
    /// </summary>
    public class EndDate
    {
        private readonly Season season = new(1, "test", new LocalDate(2021, 01, 01), new LocalDate(2021, 6, 1));

        /// <summary>
        /// A season's end date must not come before its start date.
        /// </summary>
        [Fact]
        public void EndDateMustNotBeEarlierThanStartDate()
        {
            // Arrange
            var end = season.StartDate - Period.FromDays(1);

            // Act
            var e = Assert.Throws<ArgumentOutOfRangeException>(() => season.EndDate = end);

            // Assert
            Assert.Equal("value", e.ParamName);
            Assert.Equal(end, e.ActualValue);
            Assert.StartsWith("end date must not be earlier than start date", e.Message);
        }
    }
}
