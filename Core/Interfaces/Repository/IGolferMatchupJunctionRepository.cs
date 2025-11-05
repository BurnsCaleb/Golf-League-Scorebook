using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IGolferMatchupJunctionRepository
    {
        Task<int> GetTotalGolferPointsByYear(int golferId, int year);
        Task<GolferMatchupJunction> GetById(int golferId, int matchupId);
        Task<List<GolferMatchupJunction>> GetById(List<int> golferId, List<int> matchupId);
        Task<GolferMatchupJunction> GetByGolferMatchup(int golferId, int matchupId);
        void Add(GolferMatchupJunction junction);
        Task Delete(int golferId, int matchupId);
        void Update(GolferMatchupJunction junction);
        Task SaveChanges();
    }
}
