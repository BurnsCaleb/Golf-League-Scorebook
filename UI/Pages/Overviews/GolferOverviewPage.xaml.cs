using Core.Interfaces.Service;
using Core.Models;
using InterfaceControls.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using UI.Controls;
using UI.Controls.InfoPills;

namespace UI.Pages.Overviews
{
    /// <summary>
    /// Interaction logic for GolferOverviewPage.xaml
    /// </summary>
    public partial class GolferOverviewPage : Page
    {
        public event EventHandler<GolferEvent>? Navigate;
        public event RoutedEventHandler? NewObject;

        private readonly IGolferService _golferService;
        private readonly IRoundService _roundService;

        public GolferOverviewPage()
        {
            InitializeComponent();
            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _roundService = App.ServiceProvider.GetRequiredService<IRoundService>();

            
            Loaded += GolferOverviewPage_Loaded;
        }

        private async void GolferOverviewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadGolferPills();
        }

        private async Task LoadGolferPills()
        {
            // Get Golfers
            var golfers = await _golferService.GetAll();

            if (golfers.Any())
            {
                foreach (Golfer golfer in golfers)
                {
                    GolferInfoPill golferPill = new GolferInfoPill(golfer, _roundService);
                    await golferPill.GolferVM.LoadAsyncTasks();

                    golferPill.Navigate += (sender, e) => Navigate?.Invoke(sender, e);

                    GolferPillStackPanel.Children.Add(golferPill);
                }
            }

            // Create New Golfer Pill
            string text = "Create New Golfer";
            CreateNewPill newGolferPill = new CreateNewPill(text);
            GolferPillStackPanel.Children.Add(newGolferPill);

            newGolferPill.NewObject += (sender, e) => NewObject?.Invoke(sender, e);
        }
    }
}
