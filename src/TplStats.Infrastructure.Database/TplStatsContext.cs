namespace TplStats.Infrastructure.Database
{
    using Microsoft.EntityFrameworkCore;
    using TplStats.Core.Entities;

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

#pragma warning disable CS1591, SA1600
        public DbSet<Season> Seasons { get; private set; } = null!;

        public DbSet<Team> Teams { get; private set; } = null!;
#pragma warning restore CS1591, SA1600

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseIdentityAlwaysColumns();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TplStatsContext).Assembly);
        }
    }
}
