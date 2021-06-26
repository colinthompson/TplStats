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
    /// Tests the entity type configuration for <see cref="Season"/>.
    /// </summary>
    public class SeasonConfigurationTests : EntityConfigurationTestBase<Season>
    {
        /// <summary>
        /// Verifies that required properties are indeed not nullable.
        /// </summary>
        /// <param name="propertyName">The name of a required property.</param>
        /// <param name="shouldBeNullable">Whether or not the property is expected to be nullable.</param>
        /// <param name="valueGenerated">When the property is expected to have its value generated.</param>
        [Theory]
        [InlineData(nameof(Season.Id), false)]
        [InlineData(nameof(Season.Name), false)]
        [InlineData(nameof(Season.StartDate), false)]
        [InlineData(nameof(Season.EndDate), false)]
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
            var idProp = EntityType.FindProperty(nameof(Season.Id));

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
        [InlineData(true, nameof(Season.Name))]
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

        /// <summary>
        /// Ensures that the entity's relationships are defined according to expectations.
        /// </summary>
        /// <param name="navigationName">The name of the navigation property.</param>
        /// <param name="dependentType">The dependent entity type.</param>
        /// <param name="shouldBeRequired">Whether or not the relationship is required.</param>
        [Theory]
        [InlineData(nameof(Season.Teams), typeof(Team), true)]
        public void Relationships(string navigationName, Type dependentType, bool shouldBeRequired)
        {
            // Arrange
            var principalEntityType = EntityType;
            var dependentEntityType = Model.FindEntityType(dependentType);

            // Act
            var navigation = EntityType.FindNavigation(navigationName);

            // Assert
            Assert.NotNull(navigation);
            Assert.Equal(principalEntityType, navigation.DeclaringEntityType);
            Assert.Equal(dependentEntityType, navigation.ForeignKey.DeclaringEntityType);
            Assert.Equal(shouldBeRequired, navigation.ForeignKey.IsRequired);
        }

        /// <summary>
        /// Ensures the expected check constraints are defined.
        /// </summary>
        /// <param name="name">The check constraint's name.</param>
        /// <param name="sql">The check constraint's sql statement.</param>
        [Theory]
        [InlineData("end_date_not_before_start_date", "start_date <= end_date")]
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
