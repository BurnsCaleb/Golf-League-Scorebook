using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface ISeasonRepository
    {
        Task Update(Season season);
        void Add(Season season);
        Task Delete(int seasonId);
        Task<List<Season>> GetByLeague(int leagueId);
        Task<Season> GetById(int seasonId);
        Task SaveChanges();
    }
}
