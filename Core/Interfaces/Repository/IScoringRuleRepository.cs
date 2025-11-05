using Core.Models;

namespace Core.Interfaces.Repository
{
    public interface IScoringRuleRepository
    {
        Task<ScoringRule> GetById(int scoringRuleId);
        Task<ScoringRule> GetByName(string name);
        Task<List<ScoringRule>> GetAll();
        void Add(ScoringRule scoringRule);
        void Update(ScoringRule scoringRule);
        Task Delete(int scoringRuleId);
        Task SaveChanges();
    }
}
