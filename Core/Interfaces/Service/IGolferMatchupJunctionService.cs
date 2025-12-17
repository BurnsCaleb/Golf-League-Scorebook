using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IGolferMatchupJunctionService
    {
        Task<GolferMatchupJunction> GetById(int golferId, int matchupId);
        Task<List<GolferMatchupJunction>> GetById(List<int> golferId, List<int> matchupId);
        Task<int> GetTotalGolferPointsByYear(int golferId, int year);
        Task<GolferMatchupJunction> GetByGolferMatchup(int golferId, int matchupId);
        void Add(GolferMatchupJunction junction);
        Task Delete(GolferMatchupJunction junction);
        Task Update(GolferMatchupJunction junction);
    }
}
