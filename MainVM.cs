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
            var l0 = new Layer
            {
                Name = "Background music",
                Quantity = 100,
                Distance = 900
            };
            l0.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Music\Splin-VihodaNet.mp3");
            l0.ReadyToPlay += Layer_ReadyToPlay;

            var l1 = new Layer
            {
                Name = "Surround sounds",
                Quantity = 80,
                Distance = 500
            };
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_1.mp3");
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_2.mp3");
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_3.mp3");
            l1.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Ambient\kitchen_4.mp3");
            l1.ReadyToPlay += Layer_ReadyToPlay;

            var l2 = new Layer
            {
                Name = "NatureSurround", // Voices, tree leafes, rain
                Quantity = 50,
                Distance = 300
            };
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_6.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_7.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_8.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_9.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_10.mp3");
            l2.Files.Add(@"D:\UserData\Desktop\Layers\Bar\Random\idle_11.mp3");
            l2.ReadyToPlay += Layer_ReadyToPlay;

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
            Layers.Add(l0);
            Layers.Add(l1);
            Layers.Add(l2);
            Layers.Add(l3);
        }

        public void Start()
        {
            foreach (var layer in Layers)
                layer.Played(); // Start timers after init // TODO: Change method name
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
