using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Table Name
            builder.ToTable("Course");

            // Table Constraints
            builder.ToTable(c => c.HasCheckConstraint(
                "CK_CourseSlope",
                "[CourseSlope] BETWEEN 55 AND 155"
                ));

            // Primary Key
            builder.HasKey(c => c.CourseId);

            // Properties
            builder.Property(c => c.CourseName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            builder.Property(c => c.CourseLocation)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            builder.Property(c => c.CourseRating)
                .IsRequired()
                .HasColumnType("REAL")
                .HasPrecision(4, 1);

            builder.Property(c => c.CourseSlope)
                .IsRequired()
                .HasColumnType("REAL");

            builder.Property(c => c.NumHoles)
                .IsRequired();

            builder.Property(c => c.CoursePar)
                .IsRequired();

            builder.HasMany(c => c.Holes)
                .WithOne(c => c.Course)
                .HasForeignKey(c => c.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Leagues)
                .WithOne(c => c.Course)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Rounds)
                .WithOne(c => c.Course)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(c => new { c.CourseId })
                .HasDatabaseName("IX_CourseId");
        }
    }
}
