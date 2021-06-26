namespace UnitTests.Infrastructure.Database.Configurations.Helpers
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.Extensions.DependencyInjection;
    using TplStats.Infrastructure.Database;

    /// <summary>
    /// Base class for testing entity type configurations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class EntityConfigurationTestBase<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityConfigurationTestBase{TEntity}"/> class.
        /// </summary>
        protected EntityConfigurationTestBase()
        {
            var services = new ServiceCollection();
            services.AddTplStatsDatabase("Host=localhost");
            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TplStatsContext>();

            Model = db.Model;
            EntityType = Model.FindEntityType(typeof(TEntity));
        }

        /// <summary>
        /// Gets the database model.
        /// </summary>
        protected IModel Model { get; }

        /// <summary>
        /// Gets the entity type configuration.
        /// </summary>
        protected IEntityType EntityType { get; }
    }
}
