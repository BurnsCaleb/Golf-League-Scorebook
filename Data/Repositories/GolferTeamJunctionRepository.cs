using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class GolferTeamJunctionRepository : IGolferTeamJunctionRepository
    {
        private readonly AppDbContext _context;

        public GolferTeamJunctionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(GolferTeamJunction golferTeamJunction)
        {
            _context.Add(golferTeamJunction);
        }

        public async Task Delete(int golferId, int teamId)
        {
            var junctionToDelete = await GetById(golferId, teamId);
            if (junctionToDelete != null)
            {
                _context.GolferTeamJunctions.Remove(junctionToDelete);
            }
        }

        public async Task DeleteByTeamId(int teamId)
        {
            var junctionsToDelete = await _context.GolferTeamJunctions
                .Where(j => j.TeamId == teamId)
                .ToListAsync();

            if (junctionsToDelete.Any())
            {
                _context.GolferTeamJunctions.RemoveRange(junctionsToDelete);
            }
        }

        public async Task<List<GolferTeamJunction>> GetAll()
        {
            return await _context.GolferTeamJunctions
                .ToListAsync();
        }

        public async Task<List<Golfer>> GetAllGolfersByTeam(int teamId)
        {
            return await _context.GolferTeamJunctions
                .Where(j => j.TeamId == teamId)
                .Include(j => j.Golfer)
                .Select(j => j.Golfer)
                .ToListAsync();
        }

        public async Task<List<Golfer>> GetAllGolfersByTeam(List<int> teamIds)
        {
            return await _context.GolferTeamJunctions
                .Where(j => teamIds.Contains(j.TeamId))
                .Include(j => j.Golfer)
                .Select(j => j.Golfer)
                .ToListAsync();
        }

        public async Task<GolferTeamJunction> GetById(int golferId, int teamId)
        {
            return await _context.GolferTeamJunctions
                .FindAsync(golferId, teamId);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(GolferTeamJunction golferTeamJunction)
        {
            _context.GolferTeamJunctions.Update(golferTeamJunction);
        }
    }
}
