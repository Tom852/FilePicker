using System;
using System.Collections.Specialized;
using System.IO;

namespace FilePicker
{
    public class FileChooser
    {
        public readonly StringCollection MyFiles = new StringCollection();
        private readonly Random _random = new Random();

        private static readonly string[] WatchedFolders = WatchedFoldersReader.GetFolderList();

        public FileChooser()
        {
            foreach (var folder in WatchedFolders)
            {
                if (Directory.Exists(folder))
                {
                    string[] themFiles = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                    MyFiles.AddRange(themFiles);
                }
                else
                {
                    Console.WriteLine($"Directory {folder} does not exist!"); //todo iwas log mässiges
                }
            }
        }

        public FileRepresentation Select()
        {
            FileRepresentation candidate;

            do
            {
                var index = _random.Next(MyFiles.Count);
                candidate = new FileRepresentation(MyFiles[index]);
            } while (DoWeReroll(candidate));

            return candidate;
        }

        private bool DoWeReroll(FileRepresentation candidate)
        {
            double chance = candidate.Category.GetChanceToReroll();

            double roll = _random.NextDouble();

            return roll < chance;
        }
    }
}