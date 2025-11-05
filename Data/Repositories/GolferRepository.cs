using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class GolferRepository : IGolferRepository
    {
        private readonly AppDbContext _context;

        public GolferRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Golfer golfer)
        {
            _context.Golfers.Add(golfer);
        }

        public async Task Delete(int golferId)
        {
            var golfer = await GetById(golferId);
            if (golfer != null)
            {
                _context.Golfers.Remove(golfer);
            }
        }

        public async Task<List<Golfer>> GetAll()
        {
            return await _context.Golfers
                .OrderBy(g => g.LastName)
                .ThenBy(g => g.LastName)
                .ToListAsync();
        }

        public async Task<List<Golfer>> GetAvailableSubs(int leagueId)
        {
            return await _context.GolferLeagueJunctions
                .Where(j => j.LeagueId != leagueId)
                .Select(j => j.Golfer)
                .ToListAsync();
        }

        public async Task<Golfer> GetById(int golferId)
        {
            return await _context.Golfers
                .FirstOrDefaultAsync(g => g.GolferId == golferId);
        }

        public async Task<List<Golfer>> GetByLeague(int leagueId)
        {
            return await _context.GolferLeagueJunctions
                .Where(j => j.LeagueId == leagueId)
                .Select(j => j.Golfer)
                .ToListAsync();
        }

        public async Task<Golfer> GetByName(string fullName)
        {
            return await _context.Golfers
                .FirstOrDefaultAsync(g => g.FullName.Equals(fullName));
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Golfer>> Search(string query)
        {
            query = query.Trim().ToLower();

            return await _context.Golfers.Where(g => (g.FirstName + " " + g.LastName).ToLower().Contains(query)).ToListAsync();
        }

        public void Update(Golfer golfer)
        {
            _context.Golfers.Update(golfer);
        }
    }
}
