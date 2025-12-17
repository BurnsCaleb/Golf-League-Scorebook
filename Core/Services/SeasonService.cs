using Core.DTOs.SeasonDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ISeasonRepository _seasonRepo;
        private readonly ILeagueRepository _leagueRepo;

        public SeasonService(ISeasonRepository seasonRepo, ILeagueRepository leagueRepo)
        {
            _seasonRepo = seasonRepo;
            _leagueRepo = leagueRepo;
        }

        public async Task<CreateSeasonResult> CreateSeason(string seasonName, int leagueId)
        {
            // Validate Request
            if (seasonName == null)
            {
                return CreateSeasonResult.Failure("Season Name is required.");
            }

            // Validate League Exists
            var league = await _leagueRepo.GetById(leagueId);
            if (league == null)
            {
                return CreateSeasonResult.Failure("Could not find a league.");
            }

            // Create Season
            Season season = new Season
            {
                SeasonName = seasonName,
                LeagueId = leagueId,
            };

            try
            {
                // Save to Database
                _seasonRepo.Add(season);
                await _seasonRepo.SaveChanges();

                return CreateSeasonResult.Success(season);
            }
            catch (Exception ex)
            {
                return CreateSeasonResult.Failure($"Failed creating a season: {ex.Message}");
            }
        }
    }
}
