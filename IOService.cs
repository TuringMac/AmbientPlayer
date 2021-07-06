using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientPlayer
{
    public interface IOService
    {
        string OpenFileDialog(string defaultPath = "");

        string SaveFileDialog(string defaultPath = "");
    }
}
