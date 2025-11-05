using Core.DTOs.LeagueSettingsDTOs;
using Core.Models;

namespace Core.Interfaces.Service
{
    public interface ILeagueSettingService
    {
        Task<CreateLeagueSettingResult> CreateLeagueSetting(CreateLeagueSettingRequest request);
        Task<CreateLeagueSettingResult> UpdateLeagueSetting(CreateLeagueSettingRequest request);
        Task<List<LeagueSetting>> GetLeagueSettings();
        Task<DeleteLeagueSettingResult> DeleteLeagueSetting(int settingId);
        Task<DateOnly> GetDefaultPlayDate(int leagueSettingsId);
        Task<LeagueSetting> GetById(int settingId);
        Task<List<LeagueSetting>> GetAll();
    }
}
