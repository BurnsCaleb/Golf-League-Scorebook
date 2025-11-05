using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class GolferLeagueJunctionConfiguration : IEntityTypeConfiguration<GolferLeagueJunction>
    {
        public void Configure(EntityTypeBuilder<GolferLeagueJunction> builder)
        {
            // Table Name
            builder.ToTable("GolferLeagueJunction");

            // Primary Key
            builder.HasKey(j => new {j.GolferId, j.LeagueId});

            // Properties
            builder.Property(j => j.GolferId)
                .IsRequired();

            builder.Property(j => j.LeagueId)
                .IsRequired();

            builder.HasOne(j => j.Golfer)
                .WithMany(j => j.GolferLeagueJunctions)
                .HasForeignKey(j => j.GolferId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.League)
                .WithMany(j => j.GolferLeagueJunctions)
                .HasForeignKey(j => j.LeagueId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(j => new { j.GolferId, j.LeagueId })
                .HasDatabaseName("IX_GolferLeague");
        }
    }
}
