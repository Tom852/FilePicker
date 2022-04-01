using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Settings
{
    public class PrevalenceFilter : INotifyPropertyChanged
    {
        public string FilterExpression {get; set;}
        public int RelativePrevalence {get; set;}
        public string AdditionalText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
