﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TplStats.Infrastructure.Database;

namespace TplStats.Infrastructure.Database.Migrations
{
    [DbContext(typeof(TplStatsContext))]
    partial class TplStatsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

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
#pragma warning restore 612, 618
        }
    }
}
