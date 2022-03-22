using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FilePicker.Scanner
{
    public class FileScanner
    {
        public event EventHandler<int> OnProgress;

        public IEnumerable<FileRepresentation> Scan(IEnumerable<string> folders, CancellationToken ct)
        {
            ConcurrentBag<FileRepresentation> result = new ConcurrentBag<FileRepresentation>();
            int handledFiles = 0;

            List<string> allFiles = new List<string>();

            foreach (var f in folders)
            {
                allFiles.AddRange(Directory.GetFiles(f, "*.*", SearchOption.AllDirectories));
            }

            int totalFilesCound = allFiles.Count;

            ParallelOptions o = new ParallelOptions()
            {
                CancellationToken = ct,
            };

            Parallel.For(0, totalFilesCound, o, index =>
            {
                var rep = new FileRepresentation(allFiles[index]);
                result.Add(rep);
                lock (this)
                {
                    handledFiles++;
                    if (handledFiles % 100 == 0)
                    {
                        OnProgress?.Invoke(this, 100 * handledFiles / totalFilesCound);
                    }
                }
            });

            return result;
        }
    }
}
