namespace TplStats.Infrastructure.Database.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TplStats.Core.Entities;

    /// <summary>
    /// Entity type configuration for <see cref="Game"/>.
    /// </summary>
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasCheckConstraint("end_time_not_before_start_time", "start_time <= end_time");
        }
    }
}
