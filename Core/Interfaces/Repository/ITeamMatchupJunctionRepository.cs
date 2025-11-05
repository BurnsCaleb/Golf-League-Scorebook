using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface ITeamMatchupJunctionRepository
    {
        Task<TeamMatchupJunction> GetById(int teamId, int matchupId);
        Task<List<TeamMatchupJunction>> GetAll();
        Task<List<TeamMatchupJunction>> GetByMatchup(int matchupId);
        Task<List<TeamMatchupJunction>> GetByLeague(List<int> teamIds, List<int> matchupIds);
        Task<TeamMatchupJunction> GetByMatchupTeam(int matchupId, int teamId);
        Task<List<TeamMatchupJunction>> GetByTeam(int teamId);
        Task<int> GetTotalTeamPointsByYear(int teamId, int year);
        void Add(TeamMatchupJunction teamMatchupJunction);
        void Update(TeamMatchupJunction teamMatchupJunction);
        Task Delete(int teamId, int matchupId);
        Task SaveChanges();
    }
}
