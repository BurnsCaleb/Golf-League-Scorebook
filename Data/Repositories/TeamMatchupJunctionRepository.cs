using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TeamMatchupJunctionRepository : ITeamMatchupJunctionRepository
    {
        private readonly AppDbContext _context;

        public TeamMatchupJunctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(TeamMatchupJunction teamMatchupJunction)
        {
            _context.TeamMatchupJunctions.Add(teamMatchupJunction);
        }

        public async Task Delete(int teamId, int matchupId)
        {
            var junctionToDelete = await GetById(teamId, matchupId);
            if (junctionToDelete != null)
            {
                _context.TeamMatchupJunctions.Remove(junctionToDelete);
            }
        }

        public async Task<List<TeamMatchupJunction>> GetAll()
        {
            return await _context.TeamMatchupJunctions
                .ToListAsync();
        }

        public async Task<TeamMatchupJunction> GetById(int teamId, int matchupId)
        {
            return await _context.TeamMatchupJunctions.FindAsync(teamId, matchupId);
        }

        public async Task<List<TeamMatchupJunction>> GetByLeague(List<int> teamIds, List<int> matchupIds)
        {
            return await _context.TeamMatchupJunctions
                .Where(tmj => teamIds.Contains(tmj.TeamId))
                .Where(tmj => matchupIds.Contains(tmj.MatchupId))
                .ToListAsync();
        }

        public async Task<List<TeamMatchupJunction>> GetByMatchup(int matchupId)
        {
            return await _context.TeamMatchupJunctions
                .Where(j => j.MatchupId == matchupId)
                .ToListAsync();
        }

        public async Task<List<TeamMatchupJunction>> GetByTeam(int teamId)
        {
            return await _context.TeamMatchupJunctions
                .Where(j => j.TeamId == teamId)
                .Include(j => j.Team)
                .Include(j => j.Matchup)
                .ToListAsync();
        }

        public async Task<TeamMatchupJunction> GetByMatchupTeam(int matchupId,  int teamId)
        {
            return await _context.TeamMatchupJunctions
                .FirstOrDefaultAsync(j => j.MatchupId == matchupId && j.TeamId == teamId);
        }

        public async Task<int> GetTotalTeamPointsByYear(int teamId, int seasonId)
        {
            // Get matchups for this season
            var matchupIds = await _context.Matchups
                .Where(m => m.SeasonId == seasonId)
                .Select(m => m.MatchupId)
                .ToListAsync();

            // return total points by season by team
            return await _context.TeamMatchupJunctions
                .Where(j => teamId == j.TeamId)
                .Where(j => matchupIds.Contains(j.MatchupId))
                .Select(j => j.PointsAwarded)
                .SumAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(TeamMatchupJunction teamMatchupJunction)
        {
            _context.TeamMatchupJunctions.Update(teamMatchupJunction);
        }
    }
}
