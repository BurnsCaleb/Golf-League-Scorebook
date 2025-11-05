using Core.Interfaces.Service;
using Core.Models;
using InterfaceControls.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using UI.Windows;
using ViewModels;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for ScorecardTab.xaml
    /// </summary>
    public partial class ScorecardTab : UserControl
    {
        private readonly ScorecardTabViewModel vm;
        private readonly Matchup _matchup;
        private readonly IGolferService _golferService;
        private readonly ISubstituteService _substituteService;
        private readonly int _teamId;

        public Golfer ActiveGolfer { get; set; }
        public Golfer OriginalGolfer { get; set; }

        public event EventHandler<GolferEvent>? SubstituteGolfer;
        public event EventHandler<GolferEvent>? RemoveSubstitute;
        public ScorecardTab(ScorecardTabViewModel view, Matchup matchup, int teamId)
        {
            InitializeComponent();

            _golferService = App.ServiceProvider.GetRequiredService<IGolferService>();
            _substituteService = App.ServiceProvider.GetRequiredService<ISubstituteService>();

            _matchup = matchup;
            _teamId = teamId;

            vm = view;

            Loaded += ScorecardTab_Loaded;
        }

        private async void ScorecardTab_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckForSubstitute();
            this.DataContext = vm;
        }

        private async Task CheckForSubstitute()
        {
            var originalGolfer = await _substituteService.CheckSubstitution(vm.GolferName, _matchup);

            if (originalGolfer != null)
            {
                // Show remove sub button
                RemoveSubButton.Visibility = Visibility.Visible;

                // Set original golfer
                OriginalGolfer = originalGolfer;

                // Set active golfer
                ActiveGolfer = await _golferService.GetByName(vm.GolferName);
            } else
            {
                SubstitutionButton.Visibility = Visibility.Visible;

                Golfer golfer = await _golferService.GetByName(vm.GolferName);

                OriginalGolfer = golfer;

                ActiveGolfer = golfer;
            }
        }

        private void SubstitutionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Open substitution window
            var subWindow = new SubstitutionWindow(_matchup.League, _teamId);
            subWindow.Owner = Window.GetWindow(this);

            var result = subWindow.ShowDialog();

            if (result == true)
            {
                SubstituteGolfer?.Invoke(this, new GolferEvent(subWindow.Golfer));
            }
        }

        private void RemoveSubButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveSubstitute?.Invoke(this, new GolferEvent(OriginalGolfer));
        }
    }
}
