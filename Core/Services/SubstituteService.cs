using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class SubstituteService : ISubstituteService
    {
        private readonly ISubstituteRepository _substituteRepo;
        private readonly IGolferMatchupJunctionRepository _golferMatchupJunctionRepo;
        private readonly IGolferService _golferService;
        private readonly ITeamService _teamService;

        public SubstituteService(ISubstituteRepository substituteRepo, IGolferMatchupJunctionRepository golferMatchupJunctionRepo, ITeamService teamService, IGolferService golferService)
        {
            _substituteRepo = substituteRepo;
            _golferMatchupJunctionRepo = golferMatchupJunctionRepo;
            _teamService = teamService;
            _golferService = golferService;
        }

        public async Task MakeSubstitution(Golfer oldGolfer, Golfer newGolfer, Matchup matchup)
        {
            // Replace GolferMatchupJunction with a new one
            var junctionToDelete = await _golferMatchupJunctionRepo.GetById(oldGolfer.GolferId, matchup.MatchupId);
            await _golferMatchupJunctionRepo.Delete(junctionToDelete.GolferId, junctionToDelete.MatchupId);

            // Create new Junction
            var junction = new GolferMatchupJunction
            {
                GolferId = newGolfer.GolferId,
                MatchupId = matchup.MatchupId,
            };

            _golferMatchupJunctionRepo.Add(junction);

            var team = await _teamService.GetByGolferLeague(oldGolfer.GolferId, matchup.LeagueId);

            // Create Substitution
            Substitute newSub = new Substitute
            {
                GolferId = newGolfer.GolferId,
                OriginalGolferId = oldGolfer.GolferId,
                TeamId = team.TeamId,
                MatchupId = matchup.MatchupId,
            };

            _substituteRepo.Add(newSub);
            await _substituteRepo.SaveChanges();
        }

        public async Task RemoveSubstitution(Golfer subGolfer, Matchup matchup)
        {
            // Get Substitute
            Substitute sub = await _substituteRepo.GetByGolferMatchup(subGolfer.GolferId, matchup.MatchupId);

            // Replace GolferMatchupJunction with a new one
            var junctionToDelete = await _golferMatchupJunctionRepo.GetById(subGolfer.GolferId, matchup.MatchupId);
            await _golferMatchupJunctionRepo.Delete(junctionToDelete.GolferId, junctionToDelete.MatchupId);

            // Create new Junction
            var junction = new GolferMatchupJunction
            {
                GolferId = sub.OriginalGolfer.GolferId,
                MatchupId = matchup.MatchupId,
            };

            _golferMatchupJunctionRepo.Add(junction);

            // Delete substitute record
            await _substituteRepo.Delete(sub.GolferId, sub.TeamId, sub.MatchupId);
            await _substituteRepo.SaveChanges();
        }

        public async Task<Golfer?> CheckSubstitution(string golferName, Matchup matchup)
        {
            // Get golfer
            var golfer = await _golferService.GetByName(golferName);

            // Get Substitute
            var sub = await GetByGolferMatchup(golfer.GolferId, matchup.MatchupId);

            if (sub == null)
            {
                return null;
            } else
            {
                return sub.OriginalGolfer;
            }
        }

        public async Task<Substitute> GetByGolferMatchup(int golferId, int matchupId)
        {
            return await _substituteRepo.GetByGolferMatchup(golferId, matchupId);
        }

        public async Task<List<Substitute>> GetByTeam(int teamId)
        {
            return await _substituteRepo.GetByTeam(teamId);
        }
    }
}
