using Gridify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Scanner
{
    public class FileRepresentation
    {


        public FileRepresentation() { }

        public FileRepresentation(string s)
        {
            if (!File.Exists(s))
            {
                throw new FileNotFoundException(s);
            }

            Directory = Path.GetDirectoryName(s);
            Name = Path.GetFileNameWithoutExtension(s);
            Extension = Path.GetExtension(s);
            FullPath = Path.GetFullPath(s);

            CreatedAt = File.GetCreationTime(s);
            ModifiedAt = File.GetLastWriteTime(s);
            SizeB = new FileInfo(s).Length;
        }

        public string Directory { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string FullPath { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DaysSinceCreation => (DateTime.Now - CreatedAt).Days;
        public DateTime ModifiedAt { get; set; }
        public int DaysSinceLastModified => (DateTime.Now - ModifiedAt).Days;
        public long SizeB { get; set; }
        public double SizeK => (double)SizeB / 1024;
        public double SizeM => SizeK / 1024;
        public double SizeG => SizeM / 1024;

        public bool PassesFilter(string filter)
        {
            return new FileRepresentation[] { this }.AsQueryable().ApplyFiltering(filter).Any();
        }
    }
}
