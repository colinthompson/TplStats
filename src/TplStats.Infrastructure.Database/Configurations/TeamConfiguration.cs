namespace TplStats.Infrastructure.Database.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TplStats.Core.Entities;

    /// <summary>
    /// Entity type configuration for <see cref="Team"/>.
    /// </summary>
    internal class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasIndex("SeasonId", nameof(Team.Name)).IsUnique();

            builder
                .HasMany(e => e.HomeGames)
                .WithOne(e => e.HomeTeam);
            builder
                .HasMany(e => e.AwayGames)
                .WithOne(e => e.AwayTeam);
            builder.Ignore(e => e.Games);
        }
    }
}
