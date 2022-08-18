using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;

namespace ATEMYouTubeKeyer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ViewModel = new MainViewModel();

            InitializeComponent();

            ViewModel.RetrieveDataFromYouTube();
        }

        public MainViewModel ViewModel { get; private set; }
    }
}
