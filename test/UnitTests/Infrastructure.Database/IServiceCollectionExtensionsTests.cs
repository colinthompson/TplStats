namespace UnitTests.Infrastructure.Database
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    /// <summary>
    /// Tests for <see cref="IServiceCollectionExtensions"/>.
    /// </summary>
    public class IServiceCollectionExtensionsTests
    {
        private readonly IServiceCollection services = new ServiceCollection();

        /// <summary>
        /// Services must not be null.
        /// </summary>
        [Fact]
        public void ServicesMustNotBeNull()
        {
            var e = Assert.Throws<ArgumentNullException>(
                () => IServiceCollectionExtensions.AddTplStatsDatabase(
                    null!,
                    "foo"));
            Assert.Equal("services", e.ParamName);
        }

        /// <summary>
        /// Connection string must not be null.
        /// </summary>
        [Fact]
        public void ConnectionStringMustNotBeNull()
        {
            var e = Assert.Throws<ArgumentNullException>(
                () => services.AddTplStatsDatabase(null!));
            Assert.Equal("connectionString", e.ParamName);
        }
    }
}
