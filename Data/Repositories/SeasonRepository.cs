using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly AppDbContext _context;

        public SeasonRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Season season)
        {
            _context.Seasons.Add(season);
        }

        public async Task Delete(int seasonId)
        {
            var seasonToDelete = await GetById(seasonId);
            if (seasonToDelete != null)
            {
                _context.Seasons.Remove(seasonToDelete);
            }
        }

        public async Task<Season> GetById(int seasonId)
        {
            return await _context.Seasons
                .FirstOrDefaultAsync(s => s.SeasonId == seasonId);
        }

        public async Task<List<Season>> GetByLeague(int leagueId)
        {
            return await _context.Seasons
                .Where(s => s.LeagueId == leagueId)
                .ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Season season)
        {
            _context.Seasons.Update(season);
            await SaveChanges();
        }
    }
}
