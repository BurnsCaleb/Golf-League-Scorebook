using Core.DTOs.HoleDTOs;
using Core.DTOs.ScorerDTOs;

namespace Core.Interfaces.Service
{
    public interface IScoringCalculator
    {
        PointsAwarded[] CalculatePoints(List<GolferHoleScore> scoreData);
    }
}
