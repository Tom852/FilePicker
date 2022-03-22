using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Settings
{
    public class SettingsModel
    {

        public static SettingsModel GetDefaultInstance()
        {
            return new SettingsModel()
            {
                Folders = new ObservableCollection<string>()
                {
                },
                MainFilters = new ObservableCollection<MainFilter>()
                {
                    new MainFilter()
                    {
                        FilterExpression = "Enter filter term here (see help)"
                    }
                    
                },
                Prevalences = new ObservableCollection<PrevalenceFilter>()
                {
                    new PrevalenceFilter()
                    {
                        FilterExpression = "Enter prevalence condition here (see help)",
                        RelativePrevalence = 1
                    }
                }

            };
        }
        public ObservableCollection<string> Folders { get; set; }

        public ObservableCollection<MainFilter> MainFilters { get; set; }

        public ObservableCollection<PrevalenceFilter> Prevalences { get; set; }
    }
}
