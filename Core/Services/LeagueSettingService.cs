using Core.DTOs.LeagueSettingsDTOs;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;

namespace Core.Services
{
    public class LeagueSettingService : ILeagueSettingService
    {
        private readonly ILeagueSettingRepository _settingRepo;
        private readonly IScoringRuleService _ruleService;

        public LeagueSettingService(ILeagueSettingRepository settingRepo, IScoringRuleService ruleService)
        {
            _settingRepo = settingRepo;
            _ruleService = ruleService;
        }

        public async Task<CreateLeagueSettingResult> CreateLeagueSetting(CreateLeagueSettingRequest request)
        {
            // Validate Form
            var validateErrors = await ValidateLeagueSettingRequest(request);
            if (validateErrors.Any())
            {
                return CreateLeagueSettingResult.ValidationFailure(validateErrors);
            }

            // Check for duplicate league setting
            var existingSetting = await _settingRepo.GetByName(request.LeagueSettingsName);
            if (existingSetting != null)
            {
                return CreateLeagueSettingResult.Failure(
                    $"A league setting with the name {request.LeagueSettingsName} already exists.");
            }

            // Create League Setting
            var leagueSetting = new LeagueSetting
            {
                LeagueSettingsName = request.LeagueSettingsName,
                PlayDate = request.PlayDate,
                ScoringRuleId = request.ScoringRuleId,
                TeamSize = request.TeamSize,
            };

            // Save to Database
            try
            {
                _settingRepo.Add(leagueSetting);
                await _settingRepo.SaveChanges();

                return CreateLeagueSettingResult.Success(leagueSetting);
            }
            catch (Exception ex)
            {
                return CreateLeagueSettingResult.Failure(
                    $"Failed to create league setting: {ex.Message}.");
            }
        }

        public async Task<CreateLeagueSettingResult> UpdateLeagueSetting(CreateLeagueSettingRequest request)
        {
            // Validate Form
            var validateErrors = await ValidateLeagueSettingRequest(request);
            if (validateErrors.Any())
            {
                return CreateLeagueSettingResult.ValidationFailure(validateErrors);
            }

            // Grab League to Update
            if (request.LeagueSettingsId == null)
            {
                return CreateLeagueSettingResult.Failure(
                    $"Could not find a league setting with the ID {request.LeagueSettingsId}.");
            }

            var leagueSettingToUpdate = await _settingRepo.GetById((int)request.LeagueSettingsId);

            if (leagueSettingToUpdate == null)
            {
                return CreateLeagueSettingResult.Failure(
                    $"Could not find a league setting with the ID {request.LeagueSettingsId}.");
            }

            leagueSettingToUpdate.LeagueSettingsName = request.LeagueSettingsName;
            leagueSettingToUpdate.ScoringRuleId = request.ScoringRuleId;
            leagueSettingToUpdate.TeamSize = request.TeamSize;
            leagueSettingToUpdate.PlayDate = request.PlayDate;

            // Save to Database
            try
            {
                _settingRepo.Update(leagueSettingToUpdate);
                await _settingRepo.SaveChanges();

                return CreateLeagueSettingResult.Success(leagueSettingToUpdate);
            }
            catch (Exception ex)
            {
                return CreateLeagueSettingResult.Failure(
                    $"Failed to update league setting: {ex.Message}.");
            }
        }

        public async Task<List<LeagueSetting>> GetLeagueSettings()
        {
            return await _settingRepo.GetAll();
        }

        public async Task<LeagueSetting> GetById(int settingId)
        {
            return await _settingRepo.GetById(settingId);
        }

        public async Task<DeleteLeagueSettingResult> DeleteLeagueSetting(int settingId)
        {
            try
            {
                await _settingRepo.Delete(settingId);
                await _settingRepo.SaveChanges();

                return DeleteLeagueSettingResult.Success();
            }
            catch (Exception ex)
            {
                return DeleteLeagueSettingResult.Failure($"Could not delete setting: {ex.Message}");
            }
        }

        private async Task<List<string>> ValidateLeagueSettingRequest(CreateLeagueSettingRequest request)
        {
            var errors = new List<string>();

            // Required Fields
            if (string.IsNullOrEmpty(request.LeagueSettingsName))
            {
                errors.Add("League Settings name is required.");
            }

            var scoringRule = await _ruleService.GetById(request.ScoringRuleId);
            if (scoringRule == null)
            {
                errors.Add("Scoring rule is required.");
            }

            if (request.TeamSize < 1)
            {
                errors.Add("Team size must be greater than 0.");
            }

            return errors;
        }

        public async Task<List<LeagueSetting>> GetAll()
        {
            return await _settingRepo.GetAll();
        }

        public async Task<DateOnly> GetDefaultPlayDate(int leagueSettingsId)
        {
            var stringDate = await _settingRepo.GetPlayDate(leagueSettingsId);

            // Grab Today's Date and the Day of the Week a round occurs
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DayOfWeek playDate = DateTime.Now.DayOfWeek;

            try
            {
                playDate = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), stringDate, true);
            }
            catch (ArgumentException)
            {
                throw new Exception("Error converting date of play.");
            }

            // Calculate difference
            int diff = (7 + (today.DayOfWeek - playDate)) % 7;

            return today.AddDays(-diff);
        }
    }
}
