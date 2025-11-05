using Core.DTOs.HoleDTOs;
using Core.DTOs.HoleScoreDTOs;
using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IHoleScoreService
    {
        Task<HoleScore> GetByGolferRoundHole(int golferId, int roundId, int holeId);
        Task<CreateHoleScoreResult> UpdateHoleScore(CreateHoleScoreRequest request);
        List<GolferHoleScore> PopulateScorePreviews(List<Golfer> golfers, List<GolferHoleScore> scoreData, Golfer currentGolfer, Hole currentHole);
        JumpToHoleResult JumpToHole(GolferHoleScore scoreData, List<Golfer> golfers, List<Hole> holes);
        Task<GoToPreviousHoleResult> GoToPreviousHole(Hole currentHole, List<Hole> holes, Golfer currentGolfer, List<Team> teams, List<GolferHoleScore> scoreData, Matchup matchup);
    }
}
