using Core.Models;

namespace Core.Interfaces.Service
{
    public interface IScoringRuleService
    {
        Task<List<ScoringRule>> GetAll();
        Task<ScoringRule> GetById(int ruleId);
        Task CreateScoringRules();
        IScoringCalculator CreateScorer(ScoringRule rule);
    }
}
