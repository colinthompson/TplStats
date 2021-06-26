namespace UnitTests.Infrastructure.Database.Configurations
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore.Metadata;
    using TplStats.Core.Entities;
    using UnitTests.Infrastructure.Database.Configurations.Helpers;
    using Xunit;

    /// <summary>
    /// Tests the entity type configuration for <see cref="Team"/>.
    /// </summary>
    public class TeamConfigurationTests : EntityConfigurationTestBase<Team>
    {
        /// <summary>
        /// Verifies that the entity's properties are defined according to expectations.
        /// </summary>
        /// <param name="propertyName">The name of a property.</param>
        /// <param name="shouldBeNullable">Whether or not the property is expected to be nullable.</param>
        /// <param name="valueGenerated">When the property is expected to have its value generated.</param>
        [Theory]
        [InlineData(nameof(Team.Id), false)]
        [InlineData(nameof(Team.Name), false)]
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
            var idProp = EntityType.FindProperty(nameof(Team.Id));

            // Act
            var primaryKey = EntityType.FindPrimaryKey();

            // Assert
            Assert.Single(primaryKey.Properties, idProp);
        }

        /// <summary>
        /// Ensures that the expected indexes are defined.
        /// </summary>
        /// <param name="shouldBeUnique">Whether or not the index is expected to be required to be unique.</param>
        /// <param name="propertyNames">The properties on which the index is expected to be defined.</param>
        [Theory]
        [InlineData(true, "SeasonId", nameof(Team.Name))]
        public void Indexes(bool shouldBeUnique, params string[] propertyNames)
        {
            // Arrange
            var properties = propertyNames
                .Select(name => EntityType.FindProperty(name))
                .ToList();

            // Act
            var index = EntityType.FindIndex(properties);

            // Assert
            Assert.NotNull(index);
            Assert.Equal(shouldBeUnique, index.IsUnique);
        }
    }
}
