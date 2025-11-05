using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface ITeamRepository
    {
        Task<Team> GetById(int TeamId);
        Task<Team> GetByName(string Name);
        Task<List<Team>> GetAll();
        Task<List<Team>> GetTeamsByLeague(int leagueId);
        Task<Team> GetByGolferLeague(int golferId, int leagueId);
        void Add(Team team);
        void Update(Team team);
        Task Delete(int TeamId);
        Task SaveChanges();
    }
}
