
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public partial class AppDbContext : DbContext
    {

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Golfer> Golfers { get; set; }

        public virtual DbSet<GolferLeagueJunction> GolferLeagueJunctions { get; set; }

        public virtual DbSet<GolferTeamJunction> GolferTeamJunctions { get; set; }

        public virtual DbSet<GolferMatchupJunction> GolferMatchupJunctions { get; set; }

        public virtual DbSet<Hole> Holes { get; set; }

        public virtual DbSet<League> Leagues { get; set; }

        public virtual DbSet<LeagueSetting> LeagueSettings { get; set; }

        public virtual DbSet<Matchup> Matchups { get; set; }

        public virtual DbSet<Round> Rounds { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }

        public virtual DbSet<HoleScore> HoleScores { get; set; }

        public virtual DbSet<ScoringRule> ScoringRules { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<TeamMatchupJunction> TeamMatchupJunctions { get; set; }

        public virtual DbSet<Substitute> Substitutes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
