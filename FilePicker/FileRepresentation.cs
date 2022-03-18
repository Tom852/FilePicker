using System;
using System.IO;
using System.Linq;

namespace FilePicker
{
    public class FileRepresentation
    {
        public string FilePath { get; }

        public FileRepresentation(string path)
        {
            FilePath = path;
            var paths = path.Split('\\', '/');
            RootFolder = paths[1];
            var paths3 = paths.Reverse().ToList();
            FileName = paths3[0];
            SubFolder = paths3[1];
            BaseFolder = paths3[2];

            if (BaseFolder == "Clips")
            {
                BaseFolder = SubFolder;
                SubFolder = string.Empty;
            }

            var writeDate = File.GetLastWriteTime(FilePath);
            var createDate = File.GetCreationTime(FilePath);
            Date = writeDate < createDate ? writeDate : createDate;
        }

        public string RootFolder { get; }
        public string BaseFolder { get; }
        public string SubFolder { get; }
        public string FileName { get; }
        public DateTime Date { get; }
        public int AgeInDays => (DateTime.Today - Date).Days;
        public string DateAsString => Date.ToString("ddd dd.MM.yyyy") + $" ({AgeInDays} days ago)";

        public Category Category => new Category(this);

        public bool IsPicture
        {
            get
            {
                var ext = Path.GetExtension(FilePath).ToLower();
                return ext == ".jpg" || ext == ".jpeg" || ext == ".png";
            }
        }

        public bool IsKrimsKrams
        {
            get
            {
                var ext = Path.GetExtension(FilePath).ToLower();
                return ext == ".ini" || ext == ".txt" || ext == ".ffs_db";
            }
        }

        public void Open()
        {
            System.Diagnostics.Process.Start(FilePath);
        }

        public void ShowInExplorer()
        {
            var argument = $"/e, /select, \"{FilePath}\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
    }
}