using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class LeagueSettingsRepository : ILeagueSettingRepository
    {
        private readonly AppDbContext _context;

        public LeagueSettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(LeagueSetting leagueSetting)
        {
            _context.LeagueSettings.Add(leagueSetting);
        }

        public async Task Delete(int leagueSettingsId)
        {
            var settingToDelete = await GetById(leagueSettingsId);
            if (settingToDelete != null)
            {
                _context.LeagueSettings.Remove(settingToDelete);
            }
        }

        public async Task<List<LeagueSetting>> GetAll()
        {
            return await _context.LeagueSettings
                .OrderBy(l => l.LeagueSettingsName)
                .ToListAsync();
        }

        public async Task<LeagueSetting> GetById(int leagueSettingsId)
        {
            return await _context.LeagueSettings
                .FirstOrDefaultAsync(l => l.LeagueSettingsId == leagueSettingsId);
        }

        public async Task<LeagueSetting> GetByName(string name)
        {
            return await _context.LeagueSettings
                .FirstOrDefaultAsync(l => l.LeagueSettingsName == name);
        }

        public async Task<int> GetTeamSize(int leagueSettingsId)
        {
            var leagueSettings = await GetById(leagueSettingsId);
            return leagueSettings.TeamSize;
        }

        public async Task<string> GetPlayDate(int leagueSettingsId)
        {
            var leagueSettings = await GetById(leagueSettingsId);
            return leagueSettings.PlayDate;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(LeagueSetting leagueSetting)
        {
            _context.LeagueSettings.Update(leagueSetting);
        }
    }
}
