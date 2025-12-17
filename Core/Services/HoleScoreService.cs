using Core.DTOs.HoleDTOs;
using Core.DTOs.HoleScoreDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class HoleScoreService : IHoleScoreService
    {
        private readonly IHoleScoreRepository _holeScoreRepo;
        private readonly IGolferTeamJunctionService _golferTeamJunctionService;
        private readonly ISubstituteService _substituteService;

        public HoleScoreService(IHoleScoreRepository holeScoreRepo, IGolferTeamJunctionService golferTeamJunctionService, ISubstituteService substituteService)
        {
            _holeScoreRepo = holeScoreRepo;
            _golferTeamJunctionService = golferTeamJunctionService;
            _substituteService = substituteService;
        }

        public async Task<HoleScore> GetByGolferRoundHole(int golferId, int roundId, int holeId)
        {
            return await _holeScoreRepo.GetByGolferRoundHole(golferId, roundId, holeId);
        }

        public async Task<CreateHoleScoreResult> UpdateHoleScore(CreateHoleScoreRequest request)
        {
            if (request.HoleScore == null)
            {
                return CreateHoleScoreResult.Failure("Could not find hole score.");
            }

            // grab hole score
            var holeScore = await _holeScoreRepo.GetByGolferRoundHole(request.GolferId, request.RoundId, request.HoleId);

            holeScore.GrossScore = request.GrossScore;
            holeScore.NetScore = request.NetScore;
            holeScore.DatePlayed = request.DatePlayed;

            try
            {
                _holeScoreRepo.Update(holeScore);
                _holeScoreRepo.SaveChanges();

                return CreateHoleScoreResult.Success();
            }
            catch (Exception ex)
            {
                return CreateHoleScoreResult.Failure($"Failed to update hole score: {ex.Message}.");
            }
        }

        public List<GolferHoleScore> PopulateScorePreviews(List<Golfer> golfers, List<GolferHoleScore> scoreData, Golfer currentGolfer, Hole currentHole)
        {
            var golferScores = new List<GolferHoleScore>();

            // Get other golfers 
            var otherGolfers = golfers.Where(g => g.GolferId != currentGolfer.GolferId).ToList();

            foreach (Golfer golfer in otherGolfers)
            {
                // Grab scoreData
                var golferData = scoreData
                    .Where(d => d.GolferId == golfer.GolferId)
                    .Where(d => d.HoleNumber == currentHole.HoleNum)
                    .FirstOrDefault();

                // Default to null
                var newScoreData = new GolferHoleScore
                {
                    GolferId = golfer.GolferId,
                    GolferName = golfer.FullName,
                    HoleId = currentHole.HoleId
                };

                // Overwrite if data exists
                if (golferData != null)
                {
                    newScoreData = golferData;
                }

                golferScores.Add(newScoreData);
            }

            return golferScores;
        }

        public JumpToHoleResult JumpToHole(GolferHoleScore scoreData, List<Golfer> golfers, List<Hole> holes)
        {
            // Get new current golfer
            Golfer newGolfer = golfers.First(g => g.GolferId == scoreData.GolferId);

            // Get new current hole
            Hole newHole = holes.First(h => h.HoleId == scoreData.HoleId);

            return new JumpToHoleResult
            {
                Golfer = newGolfer,
                Hole = newHole,
            };
        }

        public async Task<GoToPreviousHoleResult> GoToPreviousHole(Hole currentHole, List<Hole> holes, Golfer currentGolfer, List<Team> teams, List<GolferHoleScore> scoreData, Matchup matchup)
        {
            if (currentHole.HoleNum == 1) return GoToPreviousHoleResult.Failure("There is no previous hole.");

            // Get previous hole
            var newHole = holes
                .First(h => h.HoleNum == currentHole.HoleNum - 1);

            int newTeamId = 0;
            foreach (Team team in teams)
            {
                // Get current golfers team id
                GolferTeamJunction junction = await _golferTeamJunctionService.GetById(currentGolfer.GolferId, team.TeamId);
                if (junction != null)
                {
                    newTeamId = junction.TeamId;
                    break;
                }
            }

            if (newTeamId == 0)
            {
                // Check for substitute
                var sub = await _substituteService.GetByGolferMatchup(currentGolfer.GolferId, matchup.MatchupId);

                if (sub == null) 
                    return GoToPreviousHoleResult.Failure("Golfer does not belong to a team.");

                newTeamId = sub.TeamId;
            }

            // See if Score Data has value for this golfer for new current hole
            GolferHoleScore? existentScoreData = scoreData
                .Where(d => d.GolferId == currentGolfer.GolferId)
                .Where(d => d.HoleId == newHole.HoleId)
                .FirstOrDefault();

            if (existentScoreData != null)
            {
                return GoToPreviousHoleResult.ExistingData(existentScoreData);
            }
            else
            {
                // Set GolferHoleScore ViewModel for new hole
                var newHoleScoreData = new GolferHoleScore
                {
                    GolferId = currentGolfer.GolferId,
                    GolferName = currentGolfer.FullName,
                    TeamId = newTeamId,
                    GolferHandicap = currentGolfer.Handicap,
                    HoleId = newHole.HoleId,
                    HoleNumber = newHole.HoleNum,
                    HoleDistance = newHole.Distance,
                    HolePar = newHole.Par,
                    HoleHandicap = newHole.Handicap,
                };

                return GoToPreviousHoleResult.NewData(newHoleScoreData);
            }
        }
    }
}
