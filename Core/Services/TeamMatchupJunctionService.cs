using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class TeamMatchupJunctionService : ITeamMatchupJunctionService
    {
        private readonly ITeamMatchupJunctionRepository _teamMatchupJunctionRepo;

        public TeamMatchupJunctionService(ITeamMatchupJunctionRepository teamMatchupJunctionRepo)
        {
            _teamMatchupJunctionRepo = teamMatchupJunctionRepo;
        }

        public async Task<List<TeamMatchupJunction>> GetByLeague(List<int> teamIds, List<int> matchupIds)
        {
            return await _teamMatchupJunctionRepo.GetByLeague(teamIds, matchupIds);   
        }

        public async Task<List<TeamMatchupJunction>> GetByTeam(int teamId)
        {
            return await _teamMatchupJunctionRepo.GetByTeam(teamId);
        }

        public async Task<int> GetTotalTeamPointsByYear(int teamId, int year)
        {
            return await _teamMatchupJunctionRepo.GetTotalTeamPointsByYear(teamId, year);
        }

        public async Task<TeamMatchupJunction> GetByMatchupTeam(int matchupId, int teamId)
        {
            return await _teamMatchupJunctionRepo.GetByMatchupTeam(matchupId, teamId);
        }

        public void Add(TeamMatchupJunction teamMatchupJunction)
        {
            _teamMatchupJunctionRepo.Add(teamMatchupJunction);
        }
    }
}
