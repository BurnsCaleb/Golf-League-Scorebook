using Core.DTOs.TeamDTOs;
using Core.Models;

namespace Core.Interfaces.Service
{
    public interface ITeamService
    {
        Task<CreateTeamResult> CreateTeam(CreateTeamRequest request);
        Task<CreateTeamResult> UpdateTeam(CreateTeamRequest request);
        Task<List<Team>> GetTeamsByLeague(int leagueId);
        Task<DeleteTeamResult> Delete(int teamId);
        Task<List<Team>> GetAll();
        Task<Team> GetByGolferLeague(int golferId, int leagueId);
    }
}
