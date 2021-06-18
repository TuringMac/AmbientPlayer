using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        public MainVM()
        {
            var l1 = new Layer
            {
                Name = "AmbientMusic",
                Quantity = 100,
                Distance = 700
            };
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_1.mp3");
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_2.mp3");
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_3.mp3");
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_4.mp3");
            l1.ReadyToPlay += Layer_ReadyToPlay;
            var l2 = new Layer
            {
                Name = "NatureSurround", // Voices, tree leafes, rain
                Quantity = 70,
                Distance = 400
            };
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_6.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_7.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_8.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_9.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_10.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_11.mp3");
            var l3 = new Layer
            {
                Name = "Event",
                Quantity = 20,
                Distance = 200
            };
            l3.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_6.ogg");
            l3.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_7.ogg");
            l3.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_8.ogg");
            l3.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_9.ogg");
            l3.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_10.ogg");
            l3.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_11.ogg");
            Layers.Add(l1);
            Layers.Add(l2);
            Layers.Add(l3);
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
