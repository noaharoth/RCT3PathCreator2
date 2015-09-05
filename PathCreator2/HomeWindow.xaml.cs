using System.Windows;
using R3ALInterop;

namespace PathCreator2
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        void Callback(MOutputLog sender, string message)
        {
            MessageBox.Show(message);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
