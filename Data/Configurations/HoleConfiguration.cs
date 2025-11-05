using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class HoleConfiguration : IEntityTypeConfiguration<Hole>
    {
        public void Configure(EntityTypeBuilder<Hole> builder)
        {
            // Table Name
            builder.ToTable("Hole");

            // Table Constraints
            builder.ToTable(h => h.HasCheckConstraint(
                "CK_HoleNum",
                "[HoleNum] > 0"
                ));

            builder.ToTable(h => h.HasCheckConstraint(
                "CK_Par",
                "[Par] > 0"
                ));

            builder.ToTable(h => h.HasCheckConstraint(
                "CK_Distance",
                "[Distance] > 0"
                ));

            builder.ToTable(h => h.HasCheckConstraint(
                "CK_Handicap",
                "[Handicap] > 0"
                ));

            // Primary Key
            builder.HasKey(h => h.HoleId);

            // Properties
            builder.Property(h => h.HoleNum)
                .IsRequired();

            builder.Property(h => h.Par)
                .IsRequired();

            builder.Property(h => h.Distance)
                .IsRequired();

            builder.Property(h => h.Handicap)
                .IsRequired();

            builder.Property(h => h.CourseId)
                .IsRequired();

            builder.HasOne(h => h.Course)
                .WithMany(h => h.Holes)
                .HasForeignKey(h => h.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(h => h.HoleScores)
                .WithOne(h => h.Hole)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(h => new { h.CourseId, h.HoleId })
                .HasDatabaseName("IX_CourseHole");

            builder.HasIndex(h => h.HoleId)
                .HasDatabaseName("IX_HoleId");
        }
    }
}
