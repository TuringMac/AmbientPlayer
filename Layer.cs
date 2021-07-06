using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AmbientPlayer
{
    public enum LayerStatus
    {
        None,
        Waiting,
        Ready,
        Playing
    }

    [Serializable]
    public class Layer : INotifyPropertyChanged
    {
        public readonly TimeSpan MAX_DELAY = TimeSpan.FromMinutes(10);

        string _Name = "New layer";
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        uint _Distance = 500;
        public uint Distance
        {
            get => _Distance;
            set
            {
                if (_Distance != value)
                {
                    _Distance = value;
                    NotifyPropertyChanged();
                }
            }
        }

        uint _Quantity = 0;
        public uint Quantity
        {
            get => _Quantity;
            set
            {
                if (_Quantity != value)
                {
                    _Quantity = value;
                    NotifyPropertyChanged();
                    if (Quantity == 0)
                    {
                        tmrReady.Stop();
                    }
                    else if (Quantity == 100) // TODO: pauseless play
                    {
                        TmrReady_Tick(tmrReady, EventArgs.Empty);
                    }
                    else
                    {
                        tmrReady.Interval = new TimeSpan(MAX_DELAY.Ticks / Quantity);
                        tmrReady.Stop();
                        tmrReady.Start();
                    }
                }
            }
        }

        LayerStatus _Status = LayerStatus.None;
        [JsonIgnore]
        public LayerStatus Status
        {
            get => _Status;
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    NotifyPropertyChanged();
                    switch (_Status)
                    {
                        case LayerStatus.None:
                            tmrReady.Stop();
                            break;
                        case LayerStatus.Waiting:
                            if (Quantity < 100)
                                tmrReady.Start();
                            else
                                TmrReady_Tick(tmrReady, EventArgs.Empty);
                            break;
                        case LayerStatus.Ready:
                            ReadyToPlay?.Invoke(this, EventArgs.Empty);
                            break;
                    }
                }
            }
        }

        DispatcherTimer tmrReady = new DispatcherTimer();

        // TODO: Dirty hack
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [JsonPropertyName("Files")]
        public List<string> SerializeFiles
        {
            get => Files.ToList();
            set
            {
                foreach (var path in value)
                    Files.Add(path);
            }
        }
        [JsonIgnore]
        public ObservableCollection<string> Files { get; } = new ObservableCollection<string>();

        public event EventHandler ReadyToPlay;

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

        public Layer()
        {
            Files.CollectionChanged += Files_CollectionChanged;
            tmrReady.Interval = TimeSpan.FromMinutes(1);
            tmrReady.Tick += TmrReady_Tick;
        }

        private void Files_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Files.Count < 1)
                Status = LayerStatus.None; // Stop play when all tracks removed

            if (e.NewItems != null && e.NewItems.Count == Files.Count)
                Played(); // Run layer after first added track
        }

        private void TmrReady_Tick(object sender, EventArgs e)
        {
            tmrReady.Stop();
            Status = LayerStatus.Ready;
        }

        public void Playing()
        {
            Status = LayerStatus.Playing;
        }

        public void Played()
        {
            Status = LayerStatus.Waiting;
        }
    }
}
