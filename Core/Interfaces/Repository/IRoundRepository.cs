using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IRoundRepository
    {
        Task<Round> GetById(int roundId);
        Task<List<Round>> GetByGolferId(int golferId);
        Task<List<Round>> GetAll();
        Task<List<Round>> GetByMatchup(int matchupId);
        Task<List<Round>> GetBySeason(int season);
        Task<Round> GetByGolferMatchup(int golferId, int matchupId);

        /// <summary>
        /// Returns a golfer's average gross score for current or most recent season.
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        Task<double> GetGolferAvgGrossScore(int golferId);

        /// <summary>
        /// Returns a golfer's average net score for current or most recent season.
        /// </summary>
        /// <param name="golferId"></param>
        /// <returns></returns>
        Task<double> GetGolferAvgNetScore(int golferId);

        Task<int> GetGrossScore(int golferId, int matchupId);
        Task<int> GetNetScore(int golferId, int matchupId);

        Task<List<Round>> GetLastFortyRounds(int golferId);

        void Add(Round round);
        void Update(Round round);
        Task Delete(int roundId);
        Task SaveChanges();
    }
}
