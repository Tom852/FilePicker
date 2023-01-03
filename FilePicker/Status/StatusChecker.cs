using FilePicker.Scanner;
using FilePicker.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Status
{
    public class StatusChecker
    {
        private FilterValidator Validator { get; } = new FilterValidator();
        private FileCounter Counter { get; } = new FileCounter();

        // todo: settings und data sind iwie einfach immer parameter... würde wohl sinn ergeben, sdie global zu hinterlegen als singleton oder static :D.
        // dann könnt man settings auch bei jedem change direkt storen.
        public ApplicationStatusEnum GetStatusAfterStartup(IEnumerable<FileRepresentation> data, SettingsModel settings)
        {
            var filterStatus = Validator.Validate(settings);
            
            if (filterStatus.HasMainFilterError || filterStatus.HasPrevalenceFilterError)
            {
                return ApplicationStatusEnum.InvalidFilters;
            }

            var fcr = Counter.CountFiles(data.AsQueryable(), settings);
            if (fcr == null) // da der validator nicht alles fehler erkennt mach ich das mal so.
            {
                return ApplicationStatusEnum.InvalidFilters;
            }

            if (fcr.FileCountAfterMainFilters == 0)
            {
                return ApplicationStatusEnum.NoFiles;
            }

            if (fcr.FileCountsForEachPrevalencePool.Any(f => f == 0))
            {
                return ApplicationStatusEnum.EmptyPrevalencePools;
            }

            if (data.Any())
            {
                return ApplicationStatusEnum.OldDataFromDb;
            }

            if (!settings.Folders.Any())
            {
                return ApplicationStatusEnum.NoFolders;
            }
            
            return ApplicationStatusEnum.NoFiles;
        }

        public ApplicationStatusEnum GetStatusAfterFileScan(FileCountResult fcr)
        {
            if (fcr is null)
            {
                throw new ArgumentNullException(nameof(fcr));
            }

            // invalid filter should not be possible as can't scan with invalid filters
            if (fcr.FileCountAfterMainFilters == 0)
            {
                return ApplicationStatusEnum.NoFiles;
            }

            if (fcr.FileCountsForEachPrevalencePool.Any(f => f == 0))
            {
                return ApplicationStatusEnum.EmptyPrevalencePools;
            }

            return ApplicationStatusEnum.Ready;
        }
    }
}
