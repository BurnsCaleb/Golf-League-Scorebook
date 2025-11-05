using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private readonly AppDbContext _context;

        public RoundRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Round round)
        {
            _context.Rounds.Add(round);
        }

        public async Task Delete(int roundId)
        {
            var roundToDelete = await GetById(roundId);
            if (roundToDelete != null)
            {
                _context.Rounds.Remove(roundToDelete);
            }
        }

        public async Task<List<Round>> GetAll()
        {
            return await _context.Rounds
                .OrderBy(r => r.DatePlayed)
                .ThenByDescending(r => r.NetTotal)
                .ToListAsync();
        }

        public async Task<List<Round>> GetByGolferId(int golferId)
        {
            return await _context.Rounds
                .Where(r => r.GolferId == golferId)
                .OrderBy(r => r.DatePlayed)
                .Include(r => r.League)
                .Include(r => r.Matchup)
                .ToListAsync();
        }

        public async Task<Round> GetById(int roundId)
        {
            return await _context.Rounds
                .FirstOrDefaultAsync(r => r.RoundId == roundId);
        }

        public async Task<Round> GetByGolferMatchup(int golferId,  int matchupId)
        {
            return await _context.Rounds
                .FirstOrDefaultAsync(r => r.GolferId == golferId && r.MatchupId == matchupId);
        }

        public async Task<List<Round>> GetByMatchup(int matchupId)
        {
            return await _context.Rounds
                .Where(r => r.MatchupId == matchupId)
                .OrderByDescending(r => r.NetTotal)
                .ToListAsync();
        }

        public async Task<List<Round>> GetBySeason(int season)
        {
            return await _context.Rounds
                .Where(r => r.DatePlayed.Year == season)
                .OrderBy(r => r.DatePlayed)
                .ToListAsync();
        }

        public async Task<double> GetGolferAvgGrossScore(int golferId)
        {
            int currentYear = DateTime.Now.Year;

            return await _context.Rounds
                .Where(r => r.GolferId == golferId)
                .Where(s => s.DatePlayed.Year == currentYear)
                .Select(s => (double?)s.GrossTotal)
                .AverageAsync() ?? 0.0;
        }

        public async Task<int> GetGrossScore(int golferId, int matchupId)
        {
            return await _context.Rounds
                .Where(r => r.MatchupId == matchupId)
                .Where(r => r.GolferId == golferId)
                .Select(r => r.GrossTotal)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetNetScore(int golferId, int matchupId)
        {
            return await _context.Rounds
                .Where(r => r.MatchupId == matchupId)
                .Where(r => r.GolferId == golferId)
                .Select(r => r.NetTotal)
                .FirstOrDefaultAsync();
        }

        public async Task<double> GetGolferAvgNetScore(int golferId)
        {
            int currentYear = DateTime.Now.Year;

            return await _context.Rounds
                .Where(r => r.GolferId == golferId)
                .Where(s => s.DatePlayed.Year == currentYear)
                .Select(s => (double?)s.NetTotal)
                .AverageAsync() ?? 0.0;
        }

        public async Task<List<Round>> GetLastFortyRounds(int golferId)
        {
            return await _context.Rounds
                .Where(r => r.GolferId == golferId)
                .Where(r => r.GrossTotal != 0)
                .OrderBy(r => r.DatePlayed)
                .Take(40)
                .ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Round round)
        {
            _context.Rounds.Update(round);
        }
    }
}
