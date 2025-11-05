using Core.DTOs.SeasonDTOs;

namespace Core.Interfaces.Service
{
    public interface ISeasonService
    {
        Task<CreateSeasonResult> CreateSeason(string seasonName, int leagueId);
    }
}
