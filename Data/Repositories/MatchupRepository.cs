using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class MatchupRepository : IMatchupRepository
    {
        private readonly AppDbContext _context;

        public MatchupRepository(AppDbContext context)
        {  
            _context = context; 
        }

        public void Add(Matchup matchup)
        {
            _context.Matchups.Add(matchup);
        }

        public async Task Delete(int matchupId)
        {
            var matchupToDelete = await GetById(matchupId);
            if (matchupToDelete != null)
            {
                _context.Matchups.Remove(matchupToDelete);
            }
        }

        public async Task<Matchup> GetById(int matchupId)
        {
            return await _context.Matchups
                .FirstOrDefaultAsync(m => m.MatchupId == matchupId);
        }

        public async Task<List<Matchup>> GetByLeague(int leagueId)
        {
            return await _context.Matchups
                .Where(m => m.LeagueId == leagueId)
                .ToListAsync();
        }

        public async Task<Matchup> GetFullMatchup(int matchupId)
        {
            return await _context.Matchups
                .Include(m => m.League)
                    .ThenInclude(l => l.Course)
                        .ThenInclude(c => c.Holes)
                .Include(m => m.League)
                    .ThenInclude(l => l.LeagueSettings)
                        .ThenInclude(ls => ls.ScoringRule)
                .Include(m => m.GolferMatchupJunctions)
                    .ThenInclude(j => j.Golfer)
                .Include(m => m.Rounds)
                    .ThenInclude(r => r.Golfer)
                .Include(r => r.Rounds)
                    .ThenInclude(r => r.HoleScores)
                        .ThenInclude(hs => hs.Hole)
                .Include(m => m.TeamMatchupJunctions)
                    .ThenInclude(j => j.Team)
                        .ThenInclude(t => t.GolferTeamJunctions)
                            .ThenInclude(j => j.Golfer)
                .FirstOrDefaultAsync(m => m.MatchupId == matchupId);
        }

        public async Task<List<IGrouping<int, Matchup>>> GetYearly(int leagueId)
        {
            return await _context.Matchups
                .Where(m => m.LeagueId == leagueId)
                .Include(m => m.Season)
                .GroupBy(m => m.SeasonId)
                .ToListAsync();
        }

        public async Task<List<IGrouping<int, Matchup>>> GetWeekly(int leagueId, int year)
        {
            return await _context.Matchups
                .Where(m => m.LeagueId == leagueId)
                .Where(m => m.SeasonId == year)
                .GroupBy(m => m.Week)
                .ToListAsync();
        }

        public async Task<List<Matchup>> GetByWeek(int leagueId, int year, int week)
        {
            return await _context.Matchups
                .Where(m => m.LeagueId== leagueId)
                .Where(m => m.SeasonId == year)
                .Where(m => m.Week == week)
                .ToListAsync();
        }

        public async Task<List<Matchup>> GetByName(string name)
        {
            return await _context.Matchups
                .Where(m => m.MatchupName == name)
                .ToListAsync();
        }

        public async Task<bool> FinishedMatchups(int leagueId, int seasonId, int week)
        {
            return await _context.Matchups
                .Where(m => m.LeagueId == leagueId)
                .Where(m => m.SeasonId == seasonId)
                .Where(m => m.Week == week)
                .Where(m => m.HasPlayed == true)
                .AnyAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Matchup matchup)
        {
            _context.Matchups.Update(matchup);
        }

        public async Task<List<Matchup>> GetMatchups(int leagueId, int seasonId, int week)
        {
            return await _context.Matchups
                .Where(m => m.LeagueId == leagueId)
                .Where(m => m.SeasonId == seasonId)
                .Where(m => m.Week == week)
                .OrderBy(m => m.HasPlayed == true)
                .ThenBy(m => m.MatchupName)
                .ToListAsync();
        }

        public async Task<List<Matchup>> GetUnplayedMatchups(int leagueId, int seasonId, int week)
        {
            return await _context.Matchups
                .Where(m => m.LeagueId == leagueId)
                .Where(m => m.SeasonId == seasonId)
                .Where(m => m.Week == week)
                .Where(m => m.HasPlayed == false)
                .OrderBy(m => m.MatchupName)
                .ToListAsync();
        }

        public async Task<bool> CheckMatchups(int leagueId, int seasonId)
        {
            return await _context.Matchups
                .Where(m => m.SeasonId == seasonId)
                .Where(m => m.LeagueId == leagueId)
                .AnyAsync();
        }

        public async Task<bool> CheckMatchups(int leagueId, int seasonId, int week)
        {
            return await _context.Matchups
                .Where(m => m.SeasonId == seasonId)
                .Where(m => m.LeagueId == leagueId)
                .Where(m => m.Week == week)
                .AnyAsync();
        }

        public async Task<bool> UnfinishedMatchups(int leagueId, int week, int seasonId)
        {
            return await _context.Matchups
                .Where(m => m.SeasonId == seasonId)
                .Where(m => m.Week == week)
                .Where(m => m.LeagueId == leagueId)
                .Where(m => m.HasPlayed == false)
                .AnyAsync();
        }

        public async Task<string> GetMatchupName(int matchupId)
        {
            var matchup = await GetById(matchupId);
            if (matchup != null)
            {
                return matchup.MatchupName;
            }
            return string.Empty;
        }
    }
}
