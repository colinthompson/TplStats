namespace TplStats.Infrastructure.Database
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Database context for TPL Stats.
    /// </summary>
    public class TplStatsContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TplStatsContext"/> class.
        /// </summary>
        /// <param name="options">Database context options.</param>
        public TplStatsContext(DbContextOptions<TplStatsContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseIdentityAlwaysColumns();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TplStatsContext).Assembly);
        }
    }
}
