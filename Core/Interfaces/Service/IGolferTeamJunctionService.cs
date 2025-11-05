using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IGolferTeamJunctionService
    {
        Task<List<Golfer>> GetAllGolfersByTeam(int teamId);
    }
}
