using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class HoleScoreRepository : IHoleScoreRepository
    {
        private readonly AppDbContext _context;

        public HoleScoreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HoleScore> GetById(int golferId, int holeId)
        {
            return await _context.HoleScores
                .FindAsync(golferId, holeId);
        }

        public async Task<HoleScore> GetByGolferRoundHole(int golferId, int roundId, int holeId)
        {
            return await _context.HoleScores
                 .Where(h => h.GolferId == golferId)
                 .Where(h => h.RoundId == roundId)
                 .Where(h => h.HoleId == holeId)
                 .FirstOrDefaultAsync();
        }

        public void Update(HoleScore holeScore)
        {
            _context.HoleScores.Update(holeScore);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
