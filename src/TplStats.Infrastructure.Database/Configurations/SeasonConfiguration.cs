namespace TplStats.Infrastructure.Database.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TplStats.Core.Entities;

    /// <summary>
    /// Entity type configuration for <see cref="Season"/>.
    /// </summary>
    internal class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasIndex(e => e.Name).IsUnique();

            builder
                .HasMany(e => e.Teams)
                .WithOne()
                .IsRequired();

            builder.HasCheckConstraint("end_date_not_before_start_date", "start_date <= end_date");
        }
    }
}
