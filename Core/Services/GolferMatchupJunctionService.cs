using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using System.Threading.Tasks;

namespace Core.Services
{
    public class GolferMatchupJunctionService : IGolferMatchupJunctionService
    {
        private readonly IGolferMatchupJunctionRepository _golferMatchupJunctionRepo;

        public GolferMatchupJunctionService(IGolferMatchupJunctionRepository golferMatchupJunctionRepo)
        {
            _golferMatchupJunctionRepo = golferMatchupJunctionRepo;
        }

        public async Task<GolferMatchupJunction> GetById(int golferId, int matchupId)
        {
            return await _golferMatchupJunctionRepo.GetById(golferId, matchupId);
        }

        public async Task<List<GolferMatchupJunction>> GetById(List<int> golferId, List<int> matchupId)
        {
            return await _golferMatchupJunctionRepo.GetById(golferId, matchupId);
        }

        public async Task<int> GetTotalGolferPointsByYear(int golferId, int year)
        {
            return await _golferMatchupJunctionRepo.GetTotalGolferPointsByYear(golferId, year);
        }

        public async Task<GolferMatchupJunction> GetByGolferMatchup(int golferId, int matchupId)
        {
            return await _golferMatchupJunctionRepo.GetByGolferMatchup(golferId, matchupId);
        }

        public void Add(GolferMatchupJunction junction)
        {
            _golferMatchupJunctionRepo.Add(junction);
        }

        public async Task Delete(GolferMatchupJunction junction)
        {
            await _golferMatchupJunctionRepo.Delete(junction.GolferId, junction.MatchupId);
            await _golferMatchupJunctionRepo.SaveChanges();
        }

        public async Task Update(GolferMatchupJunction junction)
        {
            _golferMatchupJunctionRepo.Update(junction);
            await _golferMatchupJunctionRepo.SaveChanges();
        }
    }
}
