using Core.Models;

namespace Core.Interfaces.Service
{
    public interface ISubstituteService
    {
        Task<Golfer?> CheckSubstitution(string golferName, Matchup matchup);
        Task MakeSubstitution(Golfer oldGolfer, Golfer newGolfer, Matchup matchup);
        Task RemoveSubstitution(Golfer subGolfer, Matchup matchup);
        Task<Substitute> GetByGolferMatchup(int golferId, int matchupId);
        Task<List<Substitute>> GetByTeam(int teamId);
    }
}
