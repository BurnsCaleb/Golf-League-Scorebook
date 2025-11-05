using Core.Models;

namespace Core.Interfaces.Service
{
    public interface ITeamMatchupJunctionService
    {
        Task<List<TeamMatchupJunction>> GetByLeague(List<int> teamIds, List<int> matchupIds);
        Task<int> GetTotalTeamPointsByYear(int teamId, int year);
        Task<List<TeamMatchupJunction>> GetByTeam(int teamId);
        Task<TeamMatchupJunction> GetByMatchupTeam(int matchupId, int teamId);
    }
}
