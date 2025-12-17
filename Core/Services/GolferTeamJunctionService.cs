using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class GolferTeamJunctionService : IGolferTeamJunctionService
    {
        private readonly IGolferTeamJunctionRepository _golferTeamJunctionRepo;

        public GolferTeamJunctionService(IGolferTeamJunctionRepository golferTeamJunctionRepo)
        {
            _golferTeamJunctionRepo = golferTeamJunctionRepo;
        }

        public void Add(GolferTeamJunction golferTeamJunction)
        {
            _golferTeamJunctionRepo.Add(golferTeamJunction);
        }

        public async Task DeleteByTeamId(int teamId)
        {
            await _golferTeamJunctionRepo.DeleteByTeamId(teamId);
        }

        public async Task<List<Golfer>> GetAllGolfersByTeam(int teamId)
        {
            return await _golferTeamJunctionRepo.GetAllGolfersByTeam(teamId);
        }

        public async Task<List<Golfer>> GetAllGolfersByTeam(List<int> teamIds)
        {
            return await _golferTeamJunctionRepo.GetAllGolfersByTeam(teamIds);
        }

        public async Task<GolferTeamJunction> GetById(int golferId, int teamId)
        {
            return await GetById(golferId, teamId);
        }

        public async Task SaveChanges()
        {
            await _golferTeamJunctionRepo.SaveChanges();
        }
    }
}
