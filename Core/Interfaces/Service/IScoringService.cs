using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Service
{
    public interface IScoringService
    {
        int CalculateNetScore(List<int> strokesReceivedByHole, int grossScore, Hole currHole);
        List<int> DistributeStrokes(List<Hole> holes, int totalStrokes);
    }
}
