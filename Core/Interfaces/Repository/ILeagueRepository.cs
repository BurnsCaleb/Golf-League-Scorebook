using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface ILeagueRepository
    {
        Task<League> GetById(int leagueId);
        Task<League> GetByName(string name);
        Task<List<League>> GetAll();
        void Add(League league);
        void Update(League league);
        Task Delete(int leagueId);
        Task SaveChanges();
    }
}
