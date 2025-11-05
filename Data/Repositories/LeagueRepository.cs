using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly AppDbContext _context;

        public LeagueRepository(AppDbContext context) 
        {
            _context = context;
        }

        public void Add(League league)
        {
            _context.Leagues.Add(league);
        }

        public async Task Delete(int leagueId)
        {
            var league = await GetById(leagueId);
            if (league != null)
            {
                _context.Leagues.Remove(league);
            }
        }

        public async Task<List<League>> GetAll()
        {
            return await _context.Leagues
                .OrderBy(l => l.LeagueName)
                .Include(l => l.Course)
                .Include(l => l.Teams)
                .ToListAsync();
        }

        public async Task<League> GetById(int leagueId)
        {
            return await _context.Leagues
                .FirstOrDefaultAsync(l => l.LeagueId == leagueId);
        }

        public async Task<League> GetByName(string name)
        {
            return await _context.Leagues
                .FirstOrDefaultAsync(l => l.LeagueName == name);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(League league)
        {
            _context.Leagues.Update(league);
        }
    }
}
