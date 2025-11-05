using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class RoundConfiguration : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> builder)
        {
            // Table Name
            builder.ToTable("Round");

            // Primary Key
            builder.HasKey(r => r.RoundId);

            // Properties
            builder.Property(r => r.Name)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(25);

            builder.Property(r => r.NetTotal)
                .IsRequired();

            builder.Property(r => r.GrossTotal)
                .IsRequired();

            builder.Property(r => r.DatePlayed)
                .IsRequired();

            builder.Property(r => r.GolferId)
                .IsRequired();

            builder.Property(r => r.TeamId)
                .IsRequired();

            builder.Property(r => r.ActiveSubstitute)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.CourseId)
                .IsRequired();

            builder.Property(r => r.LeagueId)
                .IsRequired();

            builder.Property(r => r.MatchupId)
                .IsRequired();

            builder.HasOne(r => r.Golfer)
                .WithMany(r => r.Rounds)
                .IsRequired()
                .HasForeignKey(r => r.GolferId);

            builder.HasOne(r => r.Team)
                .WithMany(r => r.Rounds)
                .IsRequired()
                .HasForeignKey(r => r.TeamId);

            builder.HasOne(r => r.Course)
                .WithMany(r => r.Rounds)
                .IsRequired()
                .HasForeignKey(r => r.CourseId);

            builder.HasOne(r => r.League)
                .WithMany(r => r.Rounds)
                .IsRequired()
                .HasForeignKey(r => r.LeagueId);

            builder.HasOne(r => r.Matchup)
                .WithMany(r => r.Rounds)
                .IsRequired()
                .HasForeignKey(r => r.MatchupId);

            // Indexes
            builder.HasIndex(r => r.RoundId)
                .HasDatabaseName("IX_RoundId");

            builder.HasIndex(r => new { r.MatchupId, r.GolferId })
                .HasDatabaseName("IX_MatchupIdGolferId");
        }
    }
}
