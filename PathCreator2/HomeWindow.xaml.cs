// HomeWindow.xaml.cs

/*
* (C) Copyright 2015 Noah Roth
*
* All rights reserved. This program and the accompanying materials
* are made available under the terms of the GNU Lesser General Public License
* (LGPL) version 2.1 which accompanies this distribution, and is available at
* http://www.gnu.org/licenses/lgpl-2.1.html
*
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
* Lesser General Public License for more details.
*/

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
