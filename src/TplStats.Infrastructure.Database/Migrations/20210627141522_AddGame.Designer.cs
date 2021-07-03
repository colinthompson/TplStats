﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TplStats.Infrastructure.Database;

namespace TplStats.Infrastructure.Database.Migrations
{
    [DbContext(typeof(TplStatsContext))]
    [Migration("20210627141522_AddGame")]
    partial class AddGame
    {
        /// <inheritdoc/>
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            modelBuilder.Entity("TplStats.Core.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<int>("AwayTeamId")
                        .HasColumnType("integer")
                        .HasColumnName("away_team_id");

                    b.Property<LocalDateTime>("EndTime")
                        .HasColumnType("timestamp")
                        .HasColumnName("end_time");

                    b.Property<string>("Field")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("field");

                    b.Property<int>("HomeTeamId")
                        .HasColumnType("integer")
                        .HasColumnName("home_team_id");

                    b.Property<int>("SeasonId")
                        .HasColumnType("integer")
                        .HasColumnName("season_id");

                    b.Property<LocalDateTime>("StartTime")
                        .HasColumnType("timestamp")
                        .HasColumnName("start_time");

                    b.HasKey("Id")
                        .HasName("pk_games");

                    b.HasIndex("AwayTeamId")
                        .HasDatabaseName("ix_games_away_team_id");

                    b.HasIndex("HomeTeamId")
                        .HasDatabaseName("ix_games_home_team_id");

                    b.HasIndex("SeasonId")
                        .HasDatabaseName("ix_games_season_id");

                    b.ToTable("games");

                    b.HasCheckConstraint("end_time_not_before_start_time", "start_time <= end_time");
                });

            modelBuilder.Entity("TplStats.Core.Entities.Season", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<LocalDate>("EndDate")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<LocalDate>("StartDate")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.HasKey("Id")
                        .HasName("pk_seasons");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_seasons_name");

                    b.ToTable("seasons");

                    b.HasCheckConstraint("end_date_not_before_start_date", "start_date <= end_date");
                });

            modelBuilder.Entity("TplStats.Core.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("SeasonId")
                        .HasColumnType("integer")
                        .HasColumnName("season_id");

                    b.HasKey("Id")
                        .HasName("pk_teams");

                    b.HasIndex("SeasonId", "Name")
                        .IsUnique()
                        .HasDatabaseName("ix_teams_season_id_name");

                    b.ToTable("teams");
                });

            modelBuilder.Entity("TplStats.Core.Entities.Game", b =>
                {
                    b.HasOne("TplStats.Core.Entities.Team", "AwayTeam")
                        .WithMany("AwayGames")
                        .HasForeignKey("AwayTeamId")
                        .HasConstraintName("fk_games_teams_team_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TplStats.Core.Entities.Team", "HomeTeam")
                        .WithMany("HomeGames")
                        .HasForeignKey("HomeTeamId")
                        .HasConstraintName("fk_games_teams_team_id1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TplStats.Core.Entities.Season", null)
                        .WithMany("Games")
                        .HasForeignKey("SeasonId")
                        .HasConstraintName("fk_games_seasons_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");
                });

            modelBuilder.Entity("TplStats.Core.Entities.Team", b =>
                {
                    b.HasOne("TplStats.Core.Entities.Season", null)
                        .WithMany("Teams")
                        .HasForeignKey("SeasonId")
                        .HasConstraintName("fk_teams_seasons_season_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TplStats.Core.Entities.Season", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("TplStats.Core.Entities.Team", b =>
                {
                    b.Navigation("AwayGames");

                    b.Navigation("HomeGames");
                });
#pragma warning restore 612, 618
        }
    }
}