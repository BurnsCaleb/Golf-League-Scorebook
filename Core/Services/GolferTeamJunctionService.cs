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

        public async Task<List<Golfer>> GetAllGolfersByTeam(int teamId)
        {
            return await _golferTeamJunctionRepo.GetAllGolfersByTeam(teamId);
        }
    }
}
