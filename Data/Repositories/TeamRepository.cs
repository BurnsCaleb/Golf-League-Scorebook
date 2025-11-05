using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Team team)
        {
            _context.Teams.Add(team);
        }

        public async Task Delete(int TeamId)
        {
            var teamToDelete = await GetById(TeamId);
            if (teamToDelete != null)
            {
                _context.Teams.Remove(teamToDelete);
            }
        }

        public async Task<List<Team>> GetAll()
        {
            return await _context.Teams
                .OrderBy(t => t.TeamName)
                .ToListAsync();
        }

        public async Task<Team> GetById(int TeamId)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(t => t.TeamId == TeamId);
        }

        public async Task<Team> GetByName(string Name)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(t => t.TeamName == Name);
        }

        public async Task<List<Team>> GetTeamsByLeague(int leagueId)
        {
            return await _context.Teams
                .Where(t => t.LeagueId == leagueId)
                .ToListAsync();
        }

        public async Task<Team> GetByGolferLeague(int golferId, int leagueId)
        {
            var teamIds = await _context.GolferTeamJunctions
                .Where(j => j.GolferId == golferId)
                .Select(j => j.TeamId)
                .ToListAsync();

            return await _context.Teams
                .FirstOrDefaultAsync(t => teamIds.Contains(t.TeamId) && t.LeagueId == leagueId);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Team team)
        {
            _context.Teams.Update(team);
        }
    }
}
