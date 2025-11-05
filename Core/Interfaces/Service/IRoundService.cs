using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IRoundService
    {
        Task Add(Round round);
        Task Update(Round round);
        Task<List<Round>> GetByGolferId(int golferId);
        Task<double> GetGolferAvgGrossScore(int golferId);
        Task<double> GetGolferAvgNetScore(int golferId);
        Task<Round> GetById(int roundId);
        Task<Round> GetByGolferMatchup(int golferId, int matchupId);
        Task<int> GetGrossScore(int golferId, int matchupId);
        Task<int> GetNetScore(int golferId, int matchupId);
        Task<List<int>> GetLastTwentyRounds(int golferId);
    }
}
