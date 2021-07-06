using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientPlayer
{
    public class FileService : IOService
    {
        public string OpenFileDialog(string defaultPath = "")
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (!string.IsNullOrWhiteSpace(defaultPath))
                dlg.InitialDirectory = defaultPath;
            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return dlg.FileName;
            }
            return "";
        }

        public string SaveFileDialog(string defaultPath = "")
        {
            SaveFileDialog dlg = new SaveFileDialog();
            if (!string.IsNullOrWhiteSpace(defaultPath))
                dlg.InitialDirectory = defaultPath;
            var result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return dlg.FileName;
            }
            return "";
        }
    }
}
