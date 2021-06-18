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
        MediaPlayer player = new();

        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainVM();
            DataContext = vm;
            lstLayers.ItemsSource = vm.Layers;
            vm.Play += Vm_Play;

            player.MediaOpened += Player_MediaOpened;
            player.MediaFailed += Player_MediaFailed;
        }

        private void Vm_Play(Layer layer)
        {
            player.Stop();
            string filepath = layer.Files[new Random().Next(layer.Files.Count)];
            player.Volume = (1000 - layer.Distance) / 1000.0;
            player.Open(new Uri(filepath, UriKind.Absolute));
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            player.Play();
            Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Play:{player.Source?.OriginalString} Volume:{player.Volume}");
        }

        private void Player_MediaFailed(object sender, ExceptionEventArgs e)
        {
            Debug.WriteLine(e.ErrorException.Message);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            player.Stop();
            player.Close();
        }
    }
}
