using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Status
{
    public class ApplicationStatus : INotifyPropertyChanged
    {
        public ApplicationStatusEnum Status { get; set; } = ApplicationStatusEnum.Unset;
        public bool CanPlay { get =>
                Status == ApplicationStatusEnum.OldDataFromDb ||
                Status == ApplicationStatusEnum.Ready;
        }
        public bool CanScan { get =>
                Status != ApplicationStatusEnum.InvalidFilters &&
                Status != ApplicationStatusEnum.NoFolders;
        }

        public bool RequiresNewPlaylist { get; set; } = true;

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public enum ApplicationStatusEnum
    {
        Unset, Error, NoFolders, InvalidFilters, NoFiles, EmptyPrevalencePools, FilterChanged, OldDataFromDb, Ready
    }
}
