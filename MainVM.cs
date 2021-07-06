using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmbientPlayer
{
    public class MainVM : INotifyPropertyChanged
    {
        bool IsPlaying { get; set; }
        public ObservableCollection<Layer> Layers { get; } = new ObservableCollection<Layer>();
        public CommandHandler PlayPause { get; }
        public CommandHandler SaveCommand { get; }
        public CommandHandler LoadCommand { get; }
        public CommandHandler AddLayerCommand { get; }
        public CommandHandler RemoveLayerCommand { get; }
        public CommandHandler AddTrackCommand { get; }
        public CommandHandler RemoveTrackCommand { get; }

        Layer _SelectedLayer = null;
        public Layer SelectedLayer
        {
            get => _SelectedLayer;
            set
            {
                if (_SelectedLayer != value)
                {
                    _SelectedLayer = value;
                    NotifyPropertyChanged();
                }
            }
        }

        string _SelectedTrack = null;
        public string SelectedTrack
        {
            get => _SelectedTrack;
            set
            {
                if (_SelectedTrack != value)
                {
                    _SelectedTrack = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public delegate void PlayHandler(Layer layer);
        public event PlayHandler Play;
        IOService _ioService;
        bool _Started = false;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        public MainVM(IOService ioService)
        {
            _ioService = ioService;
            Layers.CollectionChanged += Layers_CollectionChanged;

            PlayPause = new CommandHandler(() => act_PlayPause(), () => true);
            SaveCommand = new CommandHandler(() => Save(), () => true);
            LoadCommand = new CommandHandler(() => Load(), () => true);

            AddLayerCommand = new CommandHandler(() => AddLayer(), () => true);
            RemoveLayerCommand = new CommandHandler(() => RemoveLayer(), () => SelectedLayer != null);
            AddTrackCommand = new CommandHandler(() => AddTrack(), () => SelectedLayer != null);
            RemoveTrackCommand = new CommandHandler(() => RemoveTrack(), () => SelectedTrack != null && SelectedLayer != null && SelectedLayer.Files.Contains(SelectedTrack));
        }

        private void Layers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is Layer layer)
                    {
                        layer.ReadyToPlay += Layer_ReadyToPlay;
                        if (_Started)
                            layer.Played();
                    }
                }
            }
        }

        public void Start()
        {
            _Started = true;
            foreach (var layer in Layers)
                layer.Played(); // Start timers after init // TODO: Change method name
        }

        public void Stop()
        {
            foreach (var layer in Layers)
                layer.Status = LayerStatus.None;
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
                }
                Start();
            }
            else
                Console.WriteLine("File not found");
        }

        public void AddLayer()
        {
            Layers.Add(new Layer());
        }

        public void RemoveLayer()
        {
            if (SelectedLayer != null)
                Layers.Remove(SelectedLayer);
        }

        public void AddTrack()
        {
            if (SelectedLayer != null)
            {
                var filename = _ioService.OpenFileDialog();
                if (!string.IsNullOrWhiteSpace(filename))
                    SelectedLayer.Files.Add(filename);
            }
        }

        public void RemoveTrack()
        {
            if (SelectedLayer != null && SelectedTrack != null && SelectedLayer.Files.Contains(SelectedTrack))
                SelectedLayer.Files.Remove(SelectedTrack);
        }

        private void Layer_ReadyToPlay(object sender, EventArgs e)
        {
            if (sender is Layer l)
                Play?.Invoke(l);
        }

        public void act_PlayPause()
        {
            if (_Started)
                Stop();
            else
                Start();
        }
    }
}
