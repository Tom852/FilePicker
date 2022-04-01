using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Settings
{
    public class MainFilter : INotifyPropertyChanged
    {
        public string FilterExpression {get; set;}
        public string AdditionalText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
