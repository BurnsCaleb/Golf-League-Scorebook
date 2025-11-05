using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Models;
using Core.DTOs.ScoringRulesDTOs;
using Core.Services.Rules;

namespace Core.Services
{
    public class ScoringRuleService : IScoringRuleService
    {
        private readonly IScoringRuleRepository _scoringRuleRepo;
        public ScoringRuleService(IScoringRuleRepository scoringRuleRepo) 
        { 
            _scoringRuleRepo = scoringRuleRepo;
        }
        public async Task<List<ScoringRule>> GetAll()
        {
            return await _scoringRuleRepo.GetAll();
        }

        public async Task<ScoringRule> GetById(int ruleId)
        {
            return await _scoringRuleRepo.GetById(ruleId);
        }

        public async Task CreateScoringRules()
        {
            bool newRule = false;

            // Check for existing scoring rules
            var teamHandicap = await _scoringRuleRepo.GetById(1);
            var strokePlay = await _scoringRuleRepo.GetById(2);
            var strokePlayHandicap = await _scoringRuleRepo.GetById(3);
            var matchPlay = await _scoringRuleRepo.GetById(4);
            var matchPlayHandicap = await _scoringRuleRepo.GetById(5);

            // Add scoring rules to database if needed
            if (teamHandicap == null)
            {
                _scoringRuleRepo.Add(ScoringRuleModels.TeamHandicap);
                newRule = true;
            }

            if (strokePlay == null)
            {
                _scoringRuleRepo.Add(ScoringRuleModels.StrokePlay);
                newRule = true;
            }

            if (strokePlayHandicap == null)
            {
                _scoringRuleRepo.Add(ScoringRuleModels.StrokePlayHandicap);
                newRule = true;
            }

            if (matchPlay == null)
            {
                _scoringRuleRepo.Add(ScoringRuleModels.MatchPlay);
                newRule = true;
            }

            if (matchPlay == null)
            {
                _scoringRuleRepo.Add(ScoringRuleModels.MatchPlayHandicap);
                newRule = true;
            }

            if (newRule == true)
            {
                await _scoringRuleRepo.SaveChanges();
            }
        }

        public IScoringCalculator CreateScorer(ScoringRule rule)
        {
            return rule.ScoringRuleId switch
            {
                1 => new TeamHandicapScorer(),
                2=> new StrokePlayScorer(),
                3 => new StrokePlayHandicapScorer(),
                4 => new MatchPlayScorer(),
                5 => new MatchPlayHandicapScorer(),
                _ => throw new ArgumentException($"Unknown scoring rule: {rule}")
            };
        }
    }
}
