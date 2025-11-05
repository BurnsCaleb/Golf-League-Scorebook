using Core.DTOs.HoleDTOs;
using Core.DTOs.MatchupDTOs;
using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IMatchupService
    {
        Task<List<IGrouping<int, Matchup>>> GetYearly(int leagueId);
        Task<List<IGrouping<int, Matchup>>> GetWeekly(int leagueId, int year);
        Task<List<Matchup>> GetByWeek(int leagueId, int year, int week);
        Task<List<Matchup>> GetMatchups(int leagueId, int season, int week);
        Task<List<Matchup>> GetUnplayedMatchups(int leagueId, int season, int week);
        Task<int> GetLatestMatchupWeek(int leagueId, int seasonId);
        Task<bool> CheckMatchups(int leagueId, int year);
        Task<bool> CheckMatchups(int leagueId, int year, int week);
        Task<bool> UnfinishedMatchups(int leagueId, int week, int year);
        Task<Matchup> GetFullMatchup(int matchupId);
        Task<EndMatchupResult> EndMatchup(EndMatchupRequest request);
        List<GolferHoleScore>? GrabMatchupScores(Matchup matchup);
        Task<string> GetMatchupName(int matchupId);
    }
}
