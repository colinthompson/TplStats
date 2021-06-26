namespace UnitTests.Core.Entities.SeasonTests
{
    using System;
    using NodaTime;
    using TplStats.Core.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="Season.Season(int, string, NodaTime.LocalDate, NodaTime.LocalDate)"/>.
    /// </summary>
    public class Constructor
    {
        /// <summary>
        /// A season's end date must not come before its start date.
        /// </summary>
        [Fact]
        public void EndDateMustNotBeEarlierThanStartDate()
        {
            // Arrange
            const int id = 42;
            const string name = "test";
            var start = new LocalDate(2021, 1, 15);
            var end = start - Period.FromDays(1);

            // Act
            var e = Assert.Throws<ArgumentOutOfRangeException>(() => new Season(id, name, start, end));

            // Assert
            Assert.Equal("endDate", e.ParamName);
            Assert.Equal(end, e.ActualValue);
            Assert.StartsWith("end date must not be earlier than start date", e.Message);
        }
    }
}
