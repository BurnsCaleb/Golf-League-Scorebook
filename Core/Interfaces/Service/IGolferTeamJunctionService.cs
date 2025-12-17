using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IGolferTeamJunctionService
    {
        Task<List<Golfer>> GetAllGolfersByTeam(int teamId);
        Task<List<Golfer>> GetAllGolfersByTeam(List<int> teamIds);
        Task<GolferTeamJunction> GetById(int golferId, int teamId);
        Task DeleteByTeamId(int teamId);
        void Add(GolferTeamJunction golferTeamJunction);
        Task SaveChanges();
    }
}
