using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class GolferMatchupJunctionRepository : IGolferMatchupJunctionRepository
    {
        private readonly AppDbContext _context;

        public GolferMatchupJunctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(GolferMatchupJunction junction)
        {
            _context.GolferMatchupJunctions.Add(junction);
        }

        public async Task Delete(int golferId, int matchupId)
        {
            var junctionToDelete = await GetById(golferId, matchupId);
            if (junctionToDelete != null)
            {
                _context.GolferMatchupJunctions.Remove(junctionToDelete);
            }
        }

        public async Task<GolferMatchupJunction> GetById(int golferId, int matchupId)
        {
            return await _context.GolferMatchupJunctions
                .FindAsync(golferId, matchupId);
        }

        public async Task<List<GolferMatchupJunction>> GetById(List<int> golferId, List<int> matchupId)
        {
            return await _context.GolferMatchupJunctions
                .Where(j => golferId.Contains(j.GolferId))
                .Where(j => matchupId.Contains(j.MatchupId))
                .ToListAsync();
        }

        public async Task<int> GetTotalGolferPointsByYear(int golferId, int seasonId)
        {
            // Get Matchups for this season
            var matchupIds = await _context.Matchups
               .Where(m => m.SeasonId == seasonId)
               .Select(m => m.MatchupId)
               .ToListAsync();

            // return total points by season by golfer
            return await _context.GolferMatchupJunctions
                .Where(j => j.GolferId == golferId)
                .Where(j => matchupIds.Contains(j.MatchupId))
                .Select(j => j.PointsAwarded)
                .SumAsync();
        }

        public async Task<GolferMatchupJunction> GetByGolferMatchup(int golferId, int matchupId)
        {
            return await _context.GolferMatchupJunctions
                .FirstOrDefaultAsync(j => j.GolferId == golferId && j.MatchupId == matchupId);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(GolferMatchupJunction junction)
        {
            _context.GolferMatchupJunctions.Update(junction);
        }
    }
}
