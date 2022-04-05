using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Scanner
{
    public class FileCountResult
    {

        public int FileCountBeforeMainFilters { get; set; }
        public int FileCountAfterMainFilters { get; set; }
        public List<int> FileCountsForEachMainFilter { get; set; } = new List<int>();
        public List<int> FileCountsForEachPrevalencePool { get; set; } = new List<int>();
    }
}
