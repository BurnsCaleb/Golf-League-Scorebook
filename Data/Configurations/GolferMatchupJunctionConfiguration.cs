using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class GolferMatchupJunctionConfiguration : IEntityTypeConfiguration<GolferMatchupJunction>
    {
        public void Configure(EntityTypeBuilder<GolferMatchupJunction> builder)
        {
            // Table Name
            builder.ToTable("GolferMatchupJunction");

            // Primary Key
            builder.HasKey(j => new { j.GolferId, j.MatchupId });

            // Properties
            builder.Property(j => j.GolferId)
                .IsRequired();

            builder.Property(j => j.MatchupId)
                .IsRequired();

            builder.Property(j => j.PointsAwarded)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(j => j.Golfer)
                .WithMany(j => j.GolferMatchupJunctions)
                .HasForeignKey(j => j.GolferId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.Matchup)
                .WithMany(j => j.GolferMatchupJunctions)
                .HasForeignKey(j => j.MatchupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(j => new { j.GolferId, j.MatchupId })
                .HasDatabaseName("IX_GolferMatchup");
        }
    }
}
