#nullable disable

using Core.Interfaces.Service;
using Core.Models;
using InterfaceControls.Events;
using System.Windows;
using System.Windows.Controls;
using ViewModels;

namespace UI.Controls.InfoPills
{
    /// <summary>
    /// Interaction logic for GolferInfoPill.xaml
    /// </summary>
    public partial class GolferInfoPill : UserControl
    {
        public event EventHandler<GolferEvent> Navigate;

        private Golfer _golfer;
        private readonly IRoundService _roundService;

        public GolferViewModel GolferVM { get; set; }
        public GolferInfoPill(Golfer golfer, IRoundService roundService)
        {
            InitializeComponent();

            _golfer = golfer;
            _roundService = roundService;

            GolferViewModel golferVM = new GolferViewModel(golfer, _roundService);
            GolferVM = golferVM;

            this.DataContext = golferVM;
        }

        private void GolferButton_Click(object sender, RoutedEventArgs e)
        {
            Navigate?.Invoke(this, new GolferEvent(_golfer));
        }
    }
}
