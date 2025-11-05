using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class GolferConfiguration : IEntityTypeConfiguration<Golfer>
    {
        public void Configure(EntityTypeBuilder<Golfer> builder)
        {
            // Table Name
            builder.ToTable("Golfer");

            // Table Constraints
            builder.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_Handicap",
                    "[Handicap] <= '54'"
                    ));

            // Primary Key
            builder.HasKey(g => g.GolferId);

            // Properties
            builder.Property(g => g.FirstName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(25);

            builder.Property(g => g.LastName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(25);

            builder.Property(g => g.Handicap)
                .IsRequired()
                .HasPrecision(4, 2)
                .HasColumnType("REAL");

            builder.Property(g => g.FullName)
                .HasComputedColumnSql("FirstName || ' ' || LastName")
                .IsRequired()
                .HasColumnType("TEXT");
                                

            builder.HasMany(g => g.GolferLeagueJunctions)
                .WithOne(glj => glj.Golfer)
                .HasForeignKey(glj => glj.GolferId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.GolferTeamJunctions)
                .WithOne(gtj => gtj.Golfer)
                .HasForeignKey(gtj => gtj.GolferId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.GolferMatchupJunctions)
                .WithOne(gmj => gmj.Golfer)
                .HasForeignKey(gmj => gmj.GolferId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.Rounds)
                .WithOne(r => r.Golfer)
                .HasForeignKey(r => r.GolferId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.HoleScores)
                .WithOne(hs => hs.Golfer)
                .HasForeignKey(hs => hs.GolferId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(g => new { g.GolferId })
                .HasDatabaseName("IX_GolferId");

            builder.HasIndex(g => new { g.LastName, g.FirstName })
                .HasDatabaseName("IX_GolferName");
        }
    }
}
