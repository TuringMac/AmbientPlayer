using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AmbientPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<Layer, MediaPlayer> Players { get; } = new();

        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainVM();
            DataContext = vm;
            lstLayers.ItemsSource = vm.Layers;
            vm.Play += Vm_Play;
            vm.Start();
        }

        private void Vm_Play(Layer layer)
        {
            MediaPlayer player;
            if (!Players.ContainsKey(layer))
            {
                player = new();
                player.MediaOpened += Player_MediaOpened;
                player.MediaFailed += Player_MediaFailed;
                player.MediaEnded += Player_MediaEnded;
                Players.Add(layer, player);
            }

            player = Players[layer];
            player.Stop();
            layer.Playing();
            string filepath = layer.Files[new Random().Next(layer.Files.Count)];
            player.Volume = (1001 - layer.Distance) / 1000.0;
            player.Open(new Uri(filepath, UriKind.Absolute));
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            if (sender is MediaPlayer player)
                foreach (var pair in Players.Where((lp) => lp.Value == player))
                {
                    player.Close();
                    pair.Key.Played();
                }
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            if (sender is MediaPlayer player)
            {
                player.Play();
                Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Play:{player.Source?.OriginalString} Volume:{player.Volume}");
            }
        }

        private void Player_MediaFailed(object sender, ExceptionEventArgs e)
        {
            Debug.WriteLine(e.ErrorException.Message);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (var player in Players.Values)
            {
                player.Stop();
                player.Close();
            }
        }
    }
}
