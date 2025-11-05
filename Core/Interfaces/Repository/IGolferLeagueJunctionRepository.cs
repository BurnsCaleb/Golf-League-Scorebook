using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IGolferLeagueJunctionRepository
    {
        Task<GolferLeagueJunction> GetById(int golferId, int leagueId);
        Task<List<GolferLeagueJunction>> GetAll();
        Task<bool> GolferExistsInLeague(int golferId, int leagueId);
        void Add(GolferLeagueJunction golferLeagueJunction);
        void Update(GolferLeagueJunction golferLeagueJunction);
        Task Delete(int golferId, int leagueId);
        Task DeleteByGolferId(int golferId);
        Task SaveChanges();
    }
}
