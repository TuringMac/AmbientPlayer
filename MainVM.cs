using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmbientPlayer
{
    public class MainVM
    {
        bool IsPlaying { get; set; }
        public ObservableCollection<Layer> Layers { get; } = new ObservableCollection<Layer>();
        public CommandHandler PlayPause { get; }
        public CommandHandler SaveCommand { get; }
        public CommandHandler LoadCommand { get; }
        public delegate void PlayHandler(Layer layer);
        public event PlayHandler Play;
        private IOService _ioService;

        public MainVM(IOService ioService)
        {
            _ioService = ioService;
            PlayPause = new CommandHandler(() => act_PlayPause(), () => true);
            SaveCommand = new CommandHandler(() => Save(), () => true);
            LoadCommand = new CommandHandler(() => Load(), () => true);

        }

        public void Start()
        {
            foreach (var layer in Layers)
                layer.Played(); // Start timers after init // TODO: Change method name
        }

        public void Save()
        {
            string filename = _ioService.SaveFileDialog();
            if (string.IsNullOrWhiteSpace(filename))
                return;
            string jsonString = JsonSerializer.Serialize(Layers, new JsonSerializerOptions() { WriteIndented = true, IgnoreReadOnlyFields = true });
            File.WriteAllText(filename, jsonString); // TODO Show user dialog
        }

        public void Load()
        {
            Layers.Clear();
            string filename = _ioService.OpenFileDialog();
            if (File.Exists(filename))
            {
                string jsonString = File.ReadAllText(filename);
                foreach (var layer in JsonSerializer.Deserialize<ObservableCollection<Layer>>(jsonString))
                {
                    Layers.Add(layer);
                    layer.ReadyToPlay += Layer_ReadyToPlay;
                }
                Start();
            }
            else
                Console.WriteLine("File not found");
        }

        private void Layer_ReadyToPlay(object sender, EventArgs e)
        {
            if (sender is Layer l)
                Play?.Invoke(l);
        }

        public void act_PlayPause()
        {

        }
    }
}
