using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class SubstituteRepository : ISubstituteRepository
    {
        private readonly AppDbContext _context;

        public SubstituteRepository(AppDbContext context)
        {  _context = context; }

        public void Add(Substitute substitute)
        {
            _context.Add(substitute);
        }

        public async Task Delete(int golferId, int teamId, int matchupId)
        {
            var substituteToDelete = await GetById(golferId, teamId, matchupId);
            if (substituteToDelete != null)
            {
                _context.Remove(substituteToDelete);
            }
        }

        public async Task<Substitute> GetById(int golferId, int teamId, int matchupId)
        {
            return await _context.Substitutes.FindAsync(golferId, teamId, matchupId);
        }

        public async Task<List<Substitute>> GetById(int golferId, int teamId)
        {
            return await _context.Substitutes
                .Where(s => s.GolferId == golferId && s.TeamId == teamId)
                .ToListAsync();
        }

        public async Task<Substitute> GetByGolferMatchup(int golferId, int matchupId)
        {
            return await _context.Substitutes
                .Include(s => s.OriginalGolfer)
                .Include(s => s.Matchup)
                .FirstOrDefaultAsync(s => s.GolferId == golferId && s.MatchupId == matchupId);
        }

        public async Task<List<Substitute>> GetByTeam(int teamId)
        {
            return await _context.Substitutes
                .Where(s => s.TeamId == teamId)
                .ToListAsync();
        }

        public async Task Update(Substitute substitute)
        {
            _context.Substitutes.Update(substitute);
            await _context.SaveChangesAsync();

        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
