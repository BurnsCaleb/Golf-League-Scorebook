using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {

        public void Configure(EntityTypeBuilder<Season> builder)
        {
            // Table Name
            builder.ToTable("Season");

            // Properties
            builder.HasKey(b => b.SeasonId);
            builder.Property(b => b.SeasonId)
                .IsRequired();

            builder.Property(b => b.SeasonName)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(50);

            builder.Property(b => b.LeagueId)
                .IsRequired();

            builder.HasMany(b => b.Matchups)
                .WithOne(b => b.Season);

            builder.HasOne(b => b.League)
                .WithMany(b => b.Seasons)
                .HasForeignKey(b => b.LeagueId)
                .IsRequired();

            // Indexes
            builder.HasIndex(b => new { b.SeasonId })
                .HasDatabaseName("IX_SeasonId");
        }
    }
}
