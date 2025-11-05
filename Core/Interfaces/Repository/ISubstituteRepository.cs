using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface ISubstituteRepository
    {
        Task<Substitute> GetById(int golferId, int teamId, int matchupId);
        Task<List<Substitute>> GetById(int golferId, int teamId);
        Task<Substitute> GetByGolferMatchup(int golferId, int matchupId);
        Task<List<Substitute>> GetByTeam(int teamId);
        void Add(Substitute substitute);
        Task Update(Substitute substitute);
        Task Delete(int golferId, int teamId, int matchupId);
        Task SaveChanges();
    }
}
