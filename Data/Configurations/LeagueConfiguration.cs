using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            // Table Name
            builder.ToTable("League");

            // Primary Key
            builder.HasKey(l => l.LeagueId);

            // Properties
            builder.Property(l => l.LeagueName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            builder.Property(l => l.LeagueSettingsId)
                .IsRequired();

            builder.Property(l => l.CourseId)
                .IsRequired();

            builder.HasMany(l => l.GolferLeagueJunctions)
                .WithOne(l => l.League)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Course)
                .WithMany(l => l.Leagues)
                .HasForeignKey(l => l.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.LeagueSettings)
                .WithMany(l => l.Leagues)
                .HasForeignKey(l => l.LeagueSettingsId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Matchups)
                .WithOne(l => l.League)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Rounds)
                .WithOne(l => l.League)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Teams)
                .WithOne(l => l.League)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(l => l.LeagueId)
                .HasDatabaseName("IX_LeagueId");
        }
    }
}
