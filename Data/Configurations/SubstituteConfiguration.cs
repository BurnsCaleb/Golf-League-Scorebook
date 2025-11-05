using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class SubstituteConfiguration : IEntityTypeConfiguration<Substitute>
    {
        public void Configure(EntityTypeBuilder<Substitute> builder)
        {
            // Table Name
            builder.ToTable("Substitute");

            // Primary Key
            builder.HasKey(s => new { s.GolferId, s.TeamId, s.MatchupId });

            // Properties
            builder.Property(s => s.GolferId)
                .IsRequired();

            builder.Property(s => s.TeamId)
                .IsRequired();

            builder.Property(s => s.MatchupId)
                .IsRequired();

            builder.HasOne(s => s.Golfer)
                .WithMany()
                .HasForeignKey(s => s.GolferId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Team)
                .WithMany(s => s.Substitutes)
                .HasForeignKey(s => s.TeamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Matchup)
                .WithMany(s => s.Substitutes)
                .HasForeignKey(s => s.MatchupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.OriginalGolfer)
                .WithMany()
                .HasForeignKey(s => s.OriginalGolferId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(s => new { s.GolferId, s.TeamId, s.MatchupId })
                .HasDatabaseName("IX_GolferTeamMatchup");

            builder.HasIndex(s => s.MatchupId)
                .HasDatabaseName("IX_Matchup");

            builder.HasIndex(s => s.OriginalGolferId)
                .HasDatabaseName("IX_OriginalGolfer");
        }
    }
}
