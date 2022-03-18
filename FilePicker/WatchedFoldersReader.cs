using System;
using System.IO;
using System.Reflection;

namespace FilePicker
{
    public static class WatchedFoldersReader
    {
        public static string TargetFile
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                string asmDirectory = Path.GetDirectoryName(path);
                return Path.Combine(asmDirectory, "../../../WatchedFolders.txt");
            }
        }

        public static string[] GetFolderList()
        {
            return File.ReadAllLines(TargetFile);
        }
    }
}