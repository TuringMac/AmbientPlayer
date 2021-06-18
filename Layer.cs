using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AmbientPlayer
{
    public class Layer : INotifyPropertyChanged
    {
        public TimeSpan MAX_DELAY { get; } = TimeSpan.FromMinutes(10);

        string _Name = "";
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

        uint _Quantity = 50;
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
                    else
                    {
                        tmrReady.Interval = MAX_DELAY / Quantity;
                        tmrReady.Start();
                    }
                }
            }
        }

        DispatcherTimer tmrReady = new DispatcherTimer();

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
            tmrReady.Interval = TimeSpan.FromMinutes(1);
            tmrReady.Tick += TmrReady_Tick;
            tmrReady.Start();
        }

        private void TmrReady_Tick(object sender, EventArgs e)
        {
            ReadyToPlay?.Invoke(this, EventArgs.Empty);
        }
    }
}
