using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface ILeagueSettingRepository
    {
        Task<LeagueSetting> GetById(int leagueSettingsId);
        Task<LeagueSetting> GetByName(string name);
        Task<List<LeagueSetting>> GetAll();
        Task<int> GetTeamSize(int leagueSettingsId);
        Task<string> GetPlayDate(int leagueSettingsId);
        void Add(LeagueSetting leagueSetting);
        void Update(LeagueSetting leagueSetting);
        Task Delete(int leagueSettingsId);
        Task SaveChanges();
    }
}
