using Core.Interfaces.Repository;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ScoringRuleRepository : IScoringRuleRepository
    {
        private readonly AppDbContext _context;

        public ScoringRuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(ScoringRule scoringRule)
        {
            _context.ScoringRules.Add(scoringRule);
        }

        public async Task Delete(int scoringRuleId)
        {
            var scoringRule = await GetById(scoringRuleId);
            if (scoringRule != null)
            {
                _context.ScoringRules.Remove(scoringRule);
            }
        }

        public async Task<List<ScoringRule>> GetAll()
        {
            return await _context.ScoringRules
                .OrderBy(s => s.RuleName)
                .ToListAsync();
        }

        public async Task<ScoringRule> GetById(int scoringRuleId)
        {
            return await _context.ScoringRules
                .FirstOrDefaultAsync(s => s.ScoringRuleId == scoringRuleId);
        }

        public async Task<ScoringRule> GetByName(string name)
        {
            return await _context.ScoringRules
                .FirstOrDefaultAsync(s => s.RuleName == name);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(ScoringRule scoringRule)
        {
            _context.ScoringRules.Update(scoringRule);
        }
    }
}
