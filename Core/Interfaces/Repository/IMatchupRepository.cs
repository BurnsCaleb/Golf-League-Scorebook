using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IMatchupRepository
    {
        Task<Matchup> GetById(int matchupId);
        Task<List<Matchup>> GetByName(string name);
        Task<List<Matchup>> GetMatchups(int leagueId, int seasonId, int week);
        Task<List<Matchup>> GetUnplayedMatchups(int leagueId, int seasonId, int week);
        Task<bool> CheckMatchups(int leagueId, int seasonId);
        Task<bool> CheckMatchups(int leagueId, int seasonId, int week);
        Task<bool> UnfinishedMatchups(int leagueId, int week, int seasonId);
        Task<bool> FinishedMatchups(int leagueId, int seasonId, int week);
        Task<List<Matchup>> GetByLeague(int leagueId);
        Task<List<IGrouping<int, Matchup>>> GetYearly(int leagueId);
        Task<List<IGrouping<int, Matchup>>> GetWeekly(int leagueId, int seasonId);
        Task<List<Matchup>> GetByWeek(int leagueId, int seasonId, int week);
        Task<string> GetMatchupName(int matchupId);
        Task<Matchup> GetFullMatchup(int matchupId);
        void Add(Matchup matchup);
        void Update(Matchup matchup);
        Task Delete(int matchupId);
        Task SaveChanges();
    }
}
