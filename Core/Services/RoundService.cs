using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class RoundService : IRoundService
    {
        private readonly IRoundRepository _roundRepo;

        public RoundService(IRoundRepository roundRepo)
        {
            _roundRepo = roundRepo;
        }

        public async Task<Round> GetById(int roundId)
        {
            return await _roundRepo.GetById(roundId);
        }

        public async Task<List<Round>> GetByGolferId(int golferId)
        {
            return await _roundRepo.GetByGolferId(golferId);
        }

        public async Task<Round> GetByGolferMatchup(int golferId, int matchupId)
        {
            return await _roundRepo.GetByGolferMatchup(golferId, matchupId);
        }

        public async Task<double> GetGolferAvgGrossScore(int golferId)
        {
            return await _roundRepo.GetGolferAvgGrossScore(golferId);
        }

        public async Task<double> GetGolferAvgNetScore(int golferId)
        {
            return await _roundRepo.GetGolferAvgNetScore(golferId);
        }

        public async Task<int> GetGrossScore(int golferId, int matchupId)
        {
            return await _roundRepo.GetGrossScore(golferId, matchupId);
        }

        public async Task<int> GetNetScore(int golferId, int matchupId)
        {
            return await _roundRepo.GetNetScore(golferId, matchupId);
        }

        public async Task<List<int>> GetLastTwentyRounds(int golferId)
        {
            // Get at most 40 rounds
            var rounds = await _roundRepo.GetLastFortyRounds(golferId);

            int nineHoleRounds = 0;

            // Get count of 9 hole rounds
            foreach (var round in rounds)
            {
                if (round.HoleScores.Count == 9)
                    nineHoleRounds++;
            }

            var roundsOut = rounds.Take(20 +  nineHoleRounds * 2).ToList();

            return roundsOut.Select(r => r.GrossTotal).ToList();

        }

        public async Task Add(Round round)
        {
            _roundRepo.Add(round);
            await _roundRepo.SaveChanges();
        }

        public async Task Update(Round round)
        {
            _roundRepo.Update(round);
            await _roundRepo.SaveChanges();
        }
    }
}
