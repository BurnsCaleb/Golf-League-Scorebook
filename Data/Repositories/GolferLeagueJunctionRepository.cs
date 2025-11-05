using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class GolferLeagueJunctionRepository : IGolferLeagueJunctionRepository
    {
        private readonly AppDbContext _context;

        public GolferLeagueJunctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(GolferLeagueJunction golferLeagueJunction)
        {
            _context.Add(golferLeagueJunction);
        }

        public async Task Delete(int golferId, int leagueId)
        {
            var junctionToDelete = await GetById(golferId, leagueId);
            if (junctionToDelete != null)
            {
                _context.GolferLeagueJunctions.Remove(junctionToDelete);
            }
        }

        public async Task DeleteByGolferId(int golferId)
        {
            var junctionsToDelete = await _context.GolferLeagueJunctions
                .Where(j => j.GolferId == golferId)
                .ToListAsync();
            if (junctionsToDelete.Any())
            {
                _context.GolferLeagueJunctions.RemoveRange(junctionsToDelete);
            }
        }

        public async Task<List<GolferLeagueJunction>> GetAll()
        {
            return await _context.GolferLeagueJunctions
                .ToListAsync();
        }

        public async Task<GolferLeagueJunction> GetById(int golferId, int leagueId)
        {
            return await _context.GolferLeagueJunctions
                .FindAsync(golferId, leagueId);
        }

        public async Task<bool> GolferExistsInLeague(int golferId, int leagueId)
        {
            return await _context.GolferLeagueJunctions
                .Where(j => j.GolferId == golferId && j.LeagueId == leagueId)
                .AnyAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(GolferLeagueJunction golferLeagueJunction)
        {
            _context.GolferLeagueJunctions.Update(golferLeagueJunction);
        }
    }
}
