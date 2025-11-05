using Core.DTOs.LeagueDTOs;
using Core.Models;

namespace Core.Interfaces.Service
{
    public interface ILeagueService
    {
        Task<CreateLeagueResult> CreateLeague(CreateLeagueRequest request);
        Task<CreateLeagueResult> UpdateLeague(CreateLeagueRequest request);
        Task<CreateLeagueScheduleResult> CreateLeagueSchedule(List<Team> teams, int leagueId, int year);
        Task<List<League>> GetAll();
        Task<League> GetById(int leagueId);
        Task<DeleteLeagueResult> Delete(int leagueId);
        Task<League> GetByName(string name);
    }
}
