using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class ScoringService : IScoringService
    {
        public int CalculateNetScore(List<int> strokesReceivedByHole, int grossScore, Hole currHole)
        {
            int strokesBack = 0;
            if (grossScore > currHole.Par)
            {
                strokesBack = strokesReceivedByHole[currHole.HoleNum - 1];
            }

            int netScore = grossScore - strokesBack;

            return netScore;
        }

        public List<int> DistributeStrokes(List<Hole> holes, int totalStrokes)
        {
            var strokes = new int[holes.Count];

            // Sort holes by handicap
            // Include original index
            var sortedHoles = holes
                .Select((hole, index) => new { Index = index, Handicap = hole.Handicap, HolePar = hole.Par })
                .OrderBy(h => h.Handicap)
                .ToList();

            int strokesRemaining = totalStrokes;
            int currentHoleIndex = 0;

            while (strokesRemaining > 0)
            {
                // Grab hole's original index
                int originalIndex = sortedHoles[currentHoleIndex].Index;

                // Distribute strokes
                strokes[originalIndex]++;
                strokesRemaining--;

                // Go to next hole or loop
                currentHoleIndex = (currentHoleIndex + 1) % sortedHoles.Count;
            }

            return strokes.ToList();
        }
    }
}
