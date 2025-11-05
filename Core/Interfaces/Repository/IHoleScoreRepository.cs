using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IHoleScoreRepository
    {
        Task<HoleScore> GetByGolferRoundHole(int golferId, int roundId, int holeId);
        Task<HoleScore> GetById(int golferId, int holeId);
        void Update(HoleScore holeScore);
        void SaveChanges();
    }
}
