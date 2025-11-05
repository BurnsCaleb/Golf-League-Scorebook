using Core.DTOs.HoleDTOs;
using InterfaceControls.Events;
using System.Windows.Controls;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for ScoreBug.xaml
    /// </summary>
    public partial class Scorebug : UserControl
    {
        private GolferHoleScore scoreData;

        public event EventHandler<GolferHoleScoreEvent>? Navigate;
        public Scorebug(GolferHoleScore scoreData)
        {
            InitializeComponent();

            this.scoreData = scoreData;

            this.DataContext = scoreData;
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Navigate?.Invoke(this, new GolferHoleScoreEvent(scoreData));
        }
    }
}
