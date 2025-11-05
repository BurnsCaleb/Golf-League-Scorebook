using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class LeagueSettingsConfiguration : IEntityTypeConfiguration<LeagueSetting>
    {
        public void Configure(EntityTypeBuilder<LeagueSetting> builder)
        {
            // Table Name
            builder.ToTable("LeagueSettings");

            // Table Constraints

            // Primary Key
            builder.HasKey(l => l.LeagueSettingsId);

            // Properties
            builder.Property(l => l.LeagueSettingsName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            builder.Property(l => l.PlayDate)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(15);

            builder.Property(l => l.ScoringRuleId)
                .IsRequired();

            builder.Property(l => l.TeamSize)
                .IsRequired();

            builder.HasMany(l => l.Leagues)
                .WithOne(l => l.LeagueSettings)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.ScoringRule)
                .WithMany(l => l.LeagueSettings)
                .HasForeignKey(l => l.ScoringRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(l => l.LeagueSettingsId)
                .HasDatabaseName("IX_LeagueSettingsId");
        }
    }
}
