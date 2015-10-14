using System.Windows;
using R3ALInterop;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace PathCreator
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {

        public HomeWindow()
        {

            RCT3AssetLibrary.Initialize(null);

            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MPath path = new MPath();

            path.IsExtended = true;

            OvlModelSearcher searcher = new OvlModelSearcher(null, path);

            var result = searcher.Search(@"C:\Users\noaha\Documents\RCT3 Custom Content\Apps\RCT3 Path Creator\My Paths\test");

            result.ShowResultAsMessageBox();
        }
    }
}
