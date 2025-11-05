using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IGolferRepository
    {
        Task<Golfer> GetById(int golferId);
        Task<Golfer> GetByName(string fullName);
        Task<List<Golfer>> GetByLeague(int leagueId);
        Task<List<Golfer>> GetAll();
        Task<List<Golfer>> Search(string query);
        Task<List<Golfer>> GetAvailableSubs(int leagueId);
        void Add(Golfer golfer);
        void Update(Golfer golfer);
        Task Delete(int golferId);
        Task SaveChanges();
    }
}
