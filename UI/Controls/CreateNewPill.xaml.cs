using System.Windows;
using System.Windows.Controls;

namespace UI.Controls
{
    /// <summary>
    /// Interaction logic for CreateNewPill.xaml
    /// </summary>
    public partial class CreateNewPill : UserControl
    {
        public RoutedEventHandler? NewObject;
        public CreateNewPill(string text)
        {
            InitializeComponent();

            CreateNewPillButton.ToolTip = text;
            HeadingTextBlock.Text = text;
        }

        private void CreateNewPillButton_Click(object sender, RoutedEventArgs e)
        {
            NewObject?.Invoke(sender, e);
        }
    }
}
