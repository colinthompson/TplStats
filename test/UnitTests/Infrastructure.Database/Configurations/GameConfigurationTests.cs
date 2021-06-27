namespace UnitTests.Infrastructure.Database.Configurations
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using TplStats.Core.Entities;
    using UnitTests.Infrastructure.Database.Configurations.Helpers;
    using Xunit;

    /// <summary>
    /// Tests the entity type configuration for <see cref="Game"/>.
    /// </summary>
    public class GameConfigurationTests : EntityConfigurationTestBase<Game>
    {
        /// <summary>
        /// Verifies that required properties are indeed not nullable.
        /// </summary>
        /// <param name="propertyName">The name of a required property.</param>
        /// <param name="shouldBeNullable">Whether or not the property is expected to be nullable.</param>
        /// <param name="valueGenerated">When the property is expected to have its value generated.</param>
        [Theory]
        [InlineData(nameof(Game.Id), false)]
        [InlineData(nameof(Game.StartTime), false)]
        [InlineData(nameof(Game.EndTime), false)]
        [InlineData(nameof(Game.Field), false)]
        public void Properties(string propertyName, bool shouldBeNullable, ValueGenerated valueGenerated = ValueGenerated.Never)
        {
            // Arrange

            // Act
            var property = EntityType.FindProperty(propertyName);

            // Assert
            Assert.NotNull(property);
            Assert.Equal(shouldBeNullable, property.IsNullable);
            Assert.Equal(valueGenerated, property.ValueGenerated);
        }

        /// <summary>
        /// Ensures the primary key is defined on the expected properties.
        /// </summary>
        [Fact]
        public void PrimaryKey()
        {
            // Arrange
            var idProp = EntityType.FindProperty(nameof(Game.Id));

            // Act
            var primaryKey = EntityType.FindPrimaryKey();

            // Assert
            Assert.Single(primaryKey.Properties, idProp);
        }

        /// <summary>
        /// Ensures the expected check constraints are defined.
        /// </summary>
        /// <param name="name">The check constraint's name.</param>
        /// <param name="sql">The check constraint's sql statement.</param>
        [Theory]
        [InlineData("end_time_not_before_start_time", "start_time <= end_time")]
        public void CheckConstraints(string name, string sql)
        {
            // Arrange

            // Act
            var constraint = EntityType.FindCheckConstraint(name);

            // Assert
            Assert.NotNull(constraint);
            Assert.Equal(sql, constraint.Sql);
        }
    }
}
