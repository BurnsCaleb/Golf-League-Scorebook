using Core.DTOs.GolferSearchBarDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class GolferSearchBarService : IGolferSearchBarService
    {
        private readonly IGolferRepository _golferRepo;
        private readonly IGolferLeagueJunctionService _golferLeagueJunctionService;

        public GolferSearchBarService(IGolferRepository golferRepo, IGolferLeagueJunctionService golferLeagueJunctionRepo)
        {
            _golferRepo = golferRepo;
            _golferLeagueJunctionService = golferLeagueJunctionRepo;
        }

        public async Task<CreateGolferSearchBarResult> PerformGolferCheck(CreateGolferSearchBarRequest request)
        {
            var golfers = new List<Golfer>();

            foreach (string name in request.GolferNames)
            {
                // Check if null
                if (string.IsNullOrEmpty(name))
                {
                    return CreateGolferSearchBarResult.Failure(
                        $"Enter a golfer name.");
                }

                // Check if golfer exists
                var golfer = await _golferRepo.GetByName(name);
                if (golfer == null)
                {
                    return CreateGolferSearchBarResult.Failure(
                        $"{name} is not a created golfer.");
                }

                // Check if golfer exists in same league
                var sameLeague = await _golferLeagueJunctionService.GolferExistsInLeague(golfer.GolferId, request.League.LeagueId);
                if (sameLeague)
                {
                    return CreateGolferSearchBarResult.Failure(
                        $"{name} already plays in this league.");
                }

                golfers.Add( golfer );
            }

            // Check for the same golfer twice
            for (int i = 0; i < request.GolferNames.Count; i++)
            {
                for (int j = 0; j < request.GolferNames.Count; j++)
                {
                    if (request.GolferNames[i] == request.GolferNames[j] && i != j)
                    {
                        return CreateGolferSearchBarResult.Failure(
                        $"A golfer cannot be multiple team members.");
                    }
                }
            }

            // Return 
            return CreateGolferSearchBarResult.Success(golfers);
        }
    }
}
