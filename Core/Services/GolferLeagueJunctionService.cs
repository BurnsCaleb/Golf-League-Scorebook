using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class GolferLeagueJunctionService : IGolferLeagueJunctionService
    {

        private readonly IGolferLeagueJunctionRepository _golferLeagueJunctionRepo;

        public GolferLeagueJunctionService(IGolferLeagueJunctionRepository golferLeagueJunctionRepo)
        {
            _golferLeagueJunctionRepo = golferLeagueJunctionRepo;
        }

        public void Add(GolferLeagueJunction golferLeagueJunction)
        {
            _golferLeagueJunctionRepo.Add(golferLeagueJunction);
        }

        public async Task DeleteByGolferId(int golferId)
        {
            await _golferLeagueJunctionRepo.DeleteByGolferId(golferId);
        }

        public async Task<bool> GolferExistsInLeague(int golferId, int leagueId)
        {
            return await _golferLeagueJunctionRepo.GolferExistsInLeague(golferId, leagueId);
        }

        public async Task SaveChanges()
        {
            await _golferLeagueJunctionRepo.SaveChanges();
        }
    }
}
