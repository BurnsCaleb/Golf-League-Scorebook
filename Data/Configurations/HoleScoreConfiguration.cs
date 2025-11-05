using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class HoleScoreConfiguration : IEntityTypeConfiguration<HoleScore>
    {
        public void Configure(EntityTypeBuilder<HoleScore> builder)
        {
            // Table Name
            builder.ToTable("HoleScore");

            // Table Constraints
            builder.ToTable(h => h.HasCheckConstraint(
                "CK_GrossScore",
                "[GrossScore] > 0"
                ));

            builder.ToTable(h => h.HasCheckConstraint(
                "CK_NetScore",
                "[NetScore] > 0"
                ));

            // Primary Key
            builder.HasKey(h => new {h.HoleId, h.GolferId, h.RoundId});

            // Properties
            builder.Property(h => h.GrossScore)
                .IsRequired();

            builder.Property(h => h.NetScore)
                .IsRequired();

            builder.Property(h => h.DatePlayed)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(h => h.GolferId)
                .IsRequired();

            builder.Property(h => h.RoundId)
                .IsRequired();

            builder.Property(h => h.HoleId)
                .IsRequired();

            builder.HasOne(h => h.Golfer)
                .WithMany(h => h.HoleScores)
                .HasForeignKey(h => h.GolferId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.Hole)
                .WithMany(h => h.HoleScores)
                .HasForeignKey(h => h.HoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.Round)
                .WithMany(h => h.HoleScores)
                .HasForeignKey(h => h.RoundId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(h => new { h.HoleId, h.GolferId, h.RoundId })
                .HasDatabaseName("IX_HoleGolferRound");
        }
    }
}
