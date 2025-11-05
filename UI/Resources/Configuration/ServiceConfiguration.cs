using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Services;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UI.Resources.Configuration
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            // Repositories
            RegisterRepositories(services);

            // Services
            RegisterServices(services);

            // UI
            RegisterUI(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IGolferLeagueJunctionRepository, GolferLeagueJunctionRepository>();
            services.AddScoped<IGolferMatchupJunctionRepository, GolferMatchupJunctionRepository>();
            services.AddScoped<IGolferRepository, GolferRepository>();
            services.AddScoped<IGolferTeamJunctionRepository, GolferTeamJunctionRepository>();
            services.AddScoped<IHoleScoreRepository, HoleScoreRepository>();
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<ILeagueSettingRepository, LeagueSettingsRepository>();
            services.AddScoped<IMatchupRepository, MatchupRepository>();
            services.AddScoped<IRoundRepository, RoundRepository>();
            services.AddScoped<IScoringRuleRepository, ScoringRuleRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<ISubstituteRepository, SubstituteRepository>();
            services.AddScoped<ITeamMatchupJunctionRepository, TeamMatchupJunctionRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IGolferMatchupJunctionService, GolferMatchupJunctionService>();
            services.AddScoped<IGolferSearchBarService, GolferSearchBarService>();
            services.AddScoped<IGolferService, GolferService>();
            services.AddScoped<IGolferTeamJunctionService, GolferTeamJunctionService>();
            services.AddScoped<IHoleService, HoleService>();
            services.AddScoped<IHoleScoreService, HoleScoreService>();
            services.AddScoped<ILeagueService, LeagueService>();
            services.AddScoped<ILeagueSettingService, LeagueSettingService>();
            services.AddScoped<IMatchupService, MatchupService>();
            services.AddScoped<IRoundService, RoundService>();
            services.AddScoped<IScoringService, ScoringService>();
            services.AddScoped<IScoringRuleService, ScoringRuleService>();
            services.AddScoped<ISeasonService, SeasonService>();
            services.AddScoped<ISubstituteService, SubstituteService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<ITeamMatchupJunctionService, TeamMatchupJunctionService>();
        }

        private static void RegisterUI(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
        }
    }
}
