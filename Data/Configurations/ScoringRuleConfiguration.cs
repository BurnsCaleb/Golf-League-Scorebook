using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class ScoringRuleConfiguration : IEntityTypeConfiguration<ScoringRule>
    {
        public void Configure(EntityTypeBuilder<ScoringRule> builder)
        {
            // Table Name
            builder.ToTable("ScoringRule");

            // Table Constraints

            // Primary Key
            builder.HasKey(s => s.ScoringRuleId);

            // Properties
            builder.Property(s => s.RuleName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            builder.Property(s => s.Description)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(250);

            builder.HasMany(s => s.LeagueSettings)
                .WithOne(s => s.ScoringRule)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(s => s.ScoringRuleId)
                .HasDatabaseName("IX_ScoringRuleId");
        }
    }
}
