using Core.Interfaces.Service;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using UI.Resources.Configuration;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            // Get Database location
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var appFolder = Path.Combine(path, "Golf League Scorebook");

            Directory.CreateDirectory(appFolder);

            string DbPath = Path.Combine(appFolder, "league_scorebook.db");

            // Configure services
            var services = new ServiceCollection();
            ServiceConfiguration.ConfigureServices(services, $"Data Source={DbPath}");
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Database initialization
            await InitializeDatabaseSetup();

            // Show main window
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private async Task InitializeDatabaseSetup()
        {
            var context = _serviceProvider.GetRequiredService<AppDbContext>();

            // Enable detailed error messages
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(60));

            try
            {
                await context.Database.MigrateAsync();
                var ruleService = _serviceProvider.GetRequiredService<IScoringRuleService>();
                await ruleService.CreateScoringRules();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw;
            }
        }

        public static IServiceProvider ServiceProvider => ((App)Current)._serviceProvider;
    }

}
