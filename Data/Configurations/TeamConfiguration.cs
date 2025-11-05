using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            // Table Name
            builder.ToTable("Team");

            // Table Constraints

            // Primary Key
            builder.HasKey(t => t.TeamId);

            // Properties
            builder.Property(t => t.TeamName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            builder.Property(t => t.LeagueId)
                .IsRequired();

            builder.HasOne(t => t.League)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.LeagueId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(t => t.TeamId)
                .HasDatabaseName("IX_TeamId");
        }
    }
}
