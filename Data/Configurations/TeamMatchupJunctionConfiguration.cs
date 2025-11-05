using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class TeamMatchupJunctionConfiguration : IEntityTypeConfiguration<TeamMatchupJunction>
    {
        public void Configure(EntityTypeBuilder<TeamMatchupJunction> builder)
        {
            // Table Name
            builder.ToTable("TeamMatchupJunction");

            // Table Constraints
            builder.ToTable(j => j.HasCheckConstraint(
                "CK_PointsAwarded",
                "[PointsAwarded] >= 0"
                ));

            // Primary Key
            builder.HasKey(j => new {j.TeamId, j.MatchupId});

            // Properties
            builder.Property(j => j.TeamId)
                .IsRequired();

            builder.Property(j => j.MatchupId)
                .IsRequired();

            builder.Property(j => j.PointsAwarded)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(j => j.Team)
                .WithMany(j => j.TeamMatchupJunctions)
                .HasForeignKey(j => j.TeamId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(j => j.Matchup)
                .WithMany(j => j.TeamMatchupJunctions)
                .HasForeignKey(j => j.MatchupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(j => new { j.TeamId, j.MatchupId })
                .HasDatabaseName("IX_TeamIdMatchupId");
        }
    }
}
