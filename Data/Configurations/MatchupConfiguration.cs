using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class MatchupConfiguration : IEntityTypeConfiguration<Matchup>
    {
        public void Configure(EntityTypeBuilder<Matchup> builder)
        {
            // Table Name
            builder.ToTable("Matchup");

            // Table Constraints

            // Primary Key
            builder.HasKey(m => m.MatchupId);

            // Properties
            builder.Property(m => m.MatchupName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(100);

            builder.Property(m => m.LeagueId)
                .IsRequired();

            builder.Property(m => m.MatchupDate)
                .IsRequired();

            builder.Property(m => m.SeasonId)
                .IsRequired();

            builder.Property(m => m.HasPlayed)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(m => m.Week)
                .IsRequired();

            builder.HasOne(m => m.League)
                .WithMany(m => m.Matchups)
                .HasForeignKey(m => m.LeagueId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Rounds)
                .WithOne(m => m.Matchup)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.TeamMatchupJunctions)
                .WithOne(m => m.Matchup)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.GolferMatchupJunctions)
                .WithOne(m => m.Matchup)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Season)
                .WithMany(m => m.Matchups)
                .HasForeignKey(m => m.SeasonId)
                .IsRequired();

            // Indexes
            builder.HasIndex(m => m.MatchupId)
                .HasDatabaseName("IX_MatchupId");

            builder.HasIndex(m => new { m.MatchupId, m.LeagueId })
                .HasDatabaseName("IX_MatchupIdLeagueId");

            builder.HasIndex(m => new { m.MatchupId, m.SeasonId })
                .HasDatabaseName("IX_MatchupIdSeasonId");
        }
    }
}
