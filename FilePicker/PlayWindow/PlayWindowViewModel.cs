using FilePicker.Scanner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.PlayWindow
{
    public class PlayWindowViewModel : INotifyPropertyChanged
    {
        public FileRepresentation PrevPrev { get; set; }
        public FileRepresentation Prev { get; set; }
        public FileRepresentation Current { get; set; }
        public FileRepresentation Next { get; set; }
        public FileRepresentation NextNext { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Rotate(FileRepresentation newFile)
        {
            PrevPrev = Prev;
            Prev = Current;
            Current = Next;
            Next = NextNext;
            NextNext = newFile;
        }
    }
}
