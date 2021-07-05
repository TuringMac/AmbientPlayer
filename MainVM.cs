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
        public PlayPauseCommand PlayPause { get; } = new PlayPauseCommand(obj => { });
        public delegate void PlayHandler(Layer layer);
        public event PlayHandler Play;

        readonly string FILE_NAME = "AmbientPlayerPreset.json";

        public MainVM()
        {
            if (File.Exists(FILE_NAME))
            {
                string jsonString = File.ReadAllText(FILE_NAME);
                Layers = JsonSerializer.Deserialize<ObservableCollection<Layer>>(jsonString);
                foreach (var layer in Layers)
                    layer.ReadyToPlay += Layer_ReadyToPlay;
            }
        }

        public void Start()
        {
            foreach (var layer in Layers)
                layer.Played(); // Start timers after init // TODO: Change method name
        }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(Layers, new JsonSerializerOptions() { WriteIndented = true, IgnoreReadOnlyFields = true });
            File.WriteAllText(FILE_NAME, jsonString); // TODO Show user dialog
        }

        private void Layer_ReadyToPlay(object sender, EventArgs e)
        {
            if (sender is Layer l)
                Play?.Invoke(l);
        }

        void act_PlayPause()
        {

        }
    }
}
