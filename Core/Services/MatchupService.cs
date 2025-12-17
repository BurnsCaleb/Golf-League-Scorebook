using Core.DTOs.HoleDTOs;
using Core.DTOs.HoleScoreDTOs;
using Core.DTOs.MatchupDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using System.Diagnostics;

namespace Core.Services
{
    public class MatchupService : IMatchupService
    {
        private readonly IMatchupRepository _matchupRepo;
        private readonly IScoringRuleService _ruleService;
        private readonly IRoundService _roundService;
        private readonly ILeagueSettingService _leagueSettingService;
        private readonly IHoleScoreService _holeScoreService;
        private readonly ITeamMatchupJunctionService _teamMatchupJunctionService;
        private readonly IGolferMatchupJunctionService _golferMatchupJunctionService;
        private readonly IGolferService _golferService;

        public MatchupService(IMatchupRepository matchupRepo, IScoringRuleService ruleService, IRoundService roundService, ILeagueSettingService leagueSettingService, IHoleScoreService holeScoreService, ITeamMatchupJunctionService teamMatchupJunctionService, IGolferMatchupJunctionService golferMatchupJunctionService, IGolferService golferService)
        {
            _matchupRepo = matchupRepo;
            _ruleService = ruleService;
            _roundService = roundService;
            _leagueSettingService = leagueSettingService;
            _holeScoreService = holeScoreService;
            _teamMatchupJunctionService = teamMatchupJunctionService;
            _golferMatchupJunctionService = golferMatchupJunctionService;
            _golferService = golferService;
        }

        public List<GolferHoleScore>? GrabMatchupScores(Matchup matchup)
        {
            var scoreData = new List<GolferHoleScore>();

            // Get Golfers in Matchup
            var golfers = matchup.Rounds
                .Select(r => r.Golfer)
                .Distinct()
                .ToList();

            foreach (Golfer golfer in golfers)
            {
                // Find Team Id
                var currTeamId = matchup.Rounds
                    .Where(r => r.Golfer == golfer)
                    .Select(r => r.TeamId)
                    .First();

                // Get golfer's round for current matchup
                var currRound = matchup.Rounds
                    .Where(r => r.MatchupId == matchup.MatchupId)
                    .Where(r => r.Golfer == golfer)
                    .First();

                // Get all hole scores for golfer's round
                var golferHoleScores = currRound.HoleScores;

                // Create GolferHoleScore
                // Add hole scores to list
                foreach (var score in golferHoleScores)
                {
                    scoreData.Add(new GolferHoleScore
                    {
                        GolferId = golfer.GolferId,
                        GolferName = golfer.FullName,
                        TeamId = currTeamId,
                        HoleId = score.HoleId,
                        HoleNumber = score.Hole.HoleNum,
                        HoleDistance = score.Hole.Distance,
                        HoleHandicap = score.Hole.Handicap,
                        HolePar = score.Hole.Par,
                        GrossScore = score.GrossScore,
                        NetScore = score.NetScore,
                        GolferHandicap = golfer.Handicap
                    });
                }
            }

            return scoreData;
        }

        public async Task<EndMatchupResult> EndMatchup(EndMatchupRequest request)
        {
            // Figure out which scoring rule to send data to
            var rule = request.Matchup.League.LeagueSettings.ScoringRule;
            var scorer = _ruleService.CreateScorer(rule);

            // Get Points
            var results = scorer.CalculatePoints(request.ScoreData);


            var rounds = new List<Round>();

            // Create and set a round for each golfer
            foreach (Golfer golfer in request.Golfers)
            {

                Round round = new Round();
                round = await _roundService.GetByGolferMatchup(golfer.GolferId, request.Matchup.MatchupId);

                if (round == null)
                {
                    // Get team Id
                    var teamId = request.ScoreData
                        .Where(d => d.GolferId == golfer.GolferId)
                        .Select(d => d.TeamId)
                        .FirstOrDefault();

                    round = new Round
                    {
                        Name = $"{request.Matchup.MatchupName} Round",
                        DatePlayed = await _leagueSettingService.GetDefaultPlayDate(request.Matchup.League.LeagueSettingsId),
                        GolferId = golfer.GolferId,
                        CourseId = request.Matchup.League.CourseId,
                        LeagueId = request.Matchup.LeagueId,
                        MatchupId = request.Matchup.MatchupId,
                        TeamId = teamId
                    };

                    // Save round to get RoundId
                    await _roundService.Add(round);
                }

                

                // Grab ScoreData for golfer
                var golferScores = request.ScoreData.Where(s => s.GolferId == golfer.GolferId).ToList();

                // Grab Gross Scores for round total
                var grossScores = golferScores.Select(s => s.GrossScore).Sum();
                if (grossScores == 0)
                {
                    throw new Exception("There was an error retrieving scores.");
                }

                round.GrossTotal = (int)grossScores;


                // Grab Net Scores for round total
                var netScores = golferScores.Select(s => s.NetScore).Sum();
                if (netScores == 0)
                {
                    throw new Exception("There was an error retrieving scores.");
                }

                round.NetTotal = (int)netScores;

                // Get Adjusted Gross Score
                // Each holes net score is at most a double bogey
                int adjustedGrossScore = 0;

                foreach (var golferScore in golferScores)
                {

                    var holeScoresToUpdate = await _holeScoreService.GetByGolferRoundHole(golferScore.GolferId, round.RoundId, golferScore.HoleId);

                    if (holeScoresToUpdate == null)
                    {
                        // Create HoleScore
                        HoleScore holeScores = new HoleScore
                        {
                            GrossScore = (int)golferScore.GrossScore,
                            NetScore = (int)golferScore.NetScore,
                            GolferId = golfer.GolferId,
                            HoleId = golferScore.HoleId,
                            DatePlayed = DateOnly.FromDateTime(DateTime.Now),
                            RoundId = round.RoundId,
                        };

                        round.HoleScores.Add(holeScores);
                    } else
                    {
                        var holeScoreRequest = new CreateHoleScoreRequest
                        {
                            HoleScore = holeScoresToUpdate,
                            GrossScore = (int)golferScore.GrossScore,
                            NetScore = (int)golferScore.NetScore,
                            GolferId = holeScoresToUpdate.GolferId,
                            RoundId = holeScoresToUpdate.RoundId,
                            HoleId = holeScoresToUpdate.HoleId,
                            DatePlayed = DateOnly.FromDateTime(DateTime.Now),
                        };

                        await _holeScoreService.UpdateHoleScore(holeScoreRequest);
                    }
                    

                    if (golferScore.GrossScore > golferScore.HolePar + 2)
                    {
                        adjustedGrossScore += golferScore.HolePar + 2;  // Cap at Double bogey
                    }
                    else
                    {
                        adjustedGrossScore += golferScore.GrossScore;     // Golfers gross score
                    }
                }

                double courseSlope = request.Matchup.League.Course.CourseSlope;
                double courseRating = request.Matchup.League.Course.CourseRating;

                // Get Score Differential
                double scoreDifferential = (113 / courseSlope) * (adjustedGrossScore - courseRating);

                if (request.Matchup.League.Course.NumHoles == 9)
                {
                    scoreDifferential = scoreDifferential * 2;
                }

                round.ScoreDifferential = scoreDifferential;

                // Add round
                rounds.Add(round);
            }


            foreach (var result in results)
            {
                if (!result.isGolfer)
                {
                    // update teamMatchupJunction
                    var junction = await _teamMatchupJunctionService.GetByMatchupTeam(request.Matchup.MatchupId, result.id);

                    junction.PointsAwarded = (int)result.points;
                }
                else
                {
                    // select golfer matchup junction for this golfer and matchup();
                    var junction = await _golferMatchupJunctionService.GetByGolferMatchup(result.id, request.Matchup.MatchupId);

                    // Update points awarded
                    junction.PointsAwarded = (int)result.points;

                    // Update GolferHandicap
                    await _golferService.UpdateGolferHandicap(result.id);
                }

            }

            // Update matchup with all rounds
            foreach (Round round in rounds)
            {
                await _roundService.Update(round);
            }

            // Set Matchup Date
            request.Matchup.MatchupDate = await _leagueSettingService.GetDefaultPlayDate(request.Matchup.League.LeagueSettingsId);

            // Update has played
            request.Matchup.HasPlayed = true;

            try
            {
                // Update Round
                _matchupRepo.Update(request.Matchup);

                await _matchupRepo.SaveChanges();

                return EndMatchupResult.Success(request.Matchup);
            }
            catch (Exception ex)
            {
                return EndMatchupResult.Failure($"Failed to update matchup: {ex.Message}.");
            }

        }

        public async Task<List<IGrouping<int, Matchup>>> GetYearly(int leagueId)
        {
            return await _matchupRepo.GetYearly(leagueId);
        }

        public async Task<List<IGrouping<int, Matchup>>> GetWeekly(int leagueId, int seasonId)
        {
            return await _matchupRepo.GetWeekly(leagueId, seasonId);
        }

        public async Task<List<Matchup>> GetByWeek(int leagueId, int seasonId, int week)
        {
            return await _matchupRepo.GetByWeek(leagueId, seasonId, week);
        }

        public async Task<List<Matchup>> GetMatchups(int leagueId, int seasonId, int week)
        {
            return await _matchupRepo.GetMatchups(leagueId, seasonId, week);
        }

        public async Task<List<Matchup>> GetUnplayedMatchups(int leagueId, int seasonId, int week)
        {
            return await _matchupRepo.GetUnplayedMatchups(leagueId, seasonId, week);
        }

        public async Task<int> GetLatestMatchupWeek(int leagueId, int seasonId)
        {
            int week = 0;
            bool hasMatchups = true;

            // Grab the last week with a finished matchup
            while(hasMatchups)
            {
                week++;

                hasMatchups = await _matchupRepo.FinishedMatchups(leagueId, seasonId, week);
            }

            week--;

            return week;
        }

        public async Task<bool> CheckMatchups(int leagueId, int seasonId)
        {
            return await _matchupRepo.CheckMatchups(leagueId, seasonId);
        }

        public async Task<bool> CheckMatchups(int leagueId, int seasonId, int week)
        {
            return await _matchupRepo.CheckMatchups(leagueId, seasonId, week);
        }

        public async Task<bool> UnfinishedMatchups(int leagueId, int week, int seasonId)
        {
            return await _matchupRepo.UnfinishedMatchups(leagueId, week, seasonId);
        }

        public async Task<Matchup> GetFullMatchup(int matchupId)
        {
            return await _matchupRepo.GetFullMatchup(matchupId);
        }

        public async Task<string> GetMatchupName(int matchupId)
        {
            return await _matchupRepo.GetMatchupName(matchupId);
        }

        public async Task Add(Matchup matchup)
        {
            _matchupRepo.Add(matchup);
            await _matchupRepo.SaveChanges();
        }
    }
}
