using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IGolferTeamJunctionRepository
    {
        Task<GolferTeamJunction> GetById(int golferId, int teamId);
        Task<List<GolferTeamJunction>> GetAll();
        Task<List<Golfer>> GetAllGolfersByTeam(int teamId);
        Task<List<Golfer>> GetAllGolfersByTeam(List<int> teamIds);
        void Add(GolferTeamJunction golferTeamJunction);
        void Update(GolferTeamJunction golferTeamJunction);
        Task Delete(int golferId, int teamId);
        Task DeleteByTeamId(int teamId);
        Task SaveChanges();
    }
}
