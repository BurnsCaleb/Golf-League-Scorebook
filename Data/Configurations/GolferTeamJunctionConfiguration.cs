using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class GolferTeamJunctionConfiguration : IEntityTypeConfiguration<GolferTeamJunction>
    {
        public void Configure(EntityTypeBuilder<GolferTeamJunction> builder)
        {
            // Table Name
            builder.ToTable("GolferTeamJunction");

            // Primary Key
            builder.HasKey(j => new { j.GolferId, j.TeamId });

            // Properties
            builder.Property(j => j.GolferId)
                .IsRequired();

            builder.Property(j => j.TeamId)
                .IsRequired();

            builder.HasOne(j => j.Golfer)
                .WithMany(j => j.GolferTeamJunctions)
                .HasForeignKey(j => j.GolferId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.Team)
                .WithMany(j => j.GolferTeamJunctions)
                .HasForeignKey(j => j.TeamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(j => new { j.GolferId, j.TeamId })
                .HasDatabaseName("IX_GolferTeam");
            
        }
    }
}
