namespace UnitTests.Web
{
    using AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using TplStats.Web;
    using Xunit;

    /// <summary>
    /// Unit tests for TPL Stats' AutoMapper configuration.
    /// </summary>
    public class AutoMapperConfigurationTests
    {
        /// <summary>
        /// Ensures the Mapper configuration is valid.
        /// </summary>
        [Fact]
        public void MapperConfigurationIsValid()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(Startup));
            using var provider = services.BuildServiceProvider();

            // Act
            var mapper = provider.GetRequiredService<IMapper>();

            // Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
