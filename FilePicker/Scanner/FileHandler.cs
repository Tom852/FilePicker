using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Scanner
{
    public static class FileHandler
    {
        public static void Open(FileRepresentation fr)
        {
            System.Diagnostics.Process.Start(fr.FullPath);
        }

        public static void ShowInExplorer(FileRepresentation fr)
        {
            var argument = $"/e, /select, \"{fr.FullPath}\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
    }
}
