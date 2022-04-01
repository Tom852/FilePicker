using FilePicker.Scanner;
using FilePicker.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gridify;

namespace FilePicker.PlayWindow
{
    public class FileChooser
    {
        public IQueryable<FileRepresentation> Data { get; }
        public SettingsModel Settings { get; }

        public FileChooser(IEnumerable<FileRepresentation> data, SettingsModel settings)
        {
            this.Data = data.AsQueryable();
            this.Settings = settings;
            foreach (var filter in settings.MainFilters)
            {
                try
                {
                    // todo: service, ist dry mit count anziegen.
                    // todo: builder nutzen, wie bei count, ist effizienter
                    this.Data = this.Data.ApplyFiltering(filter.FilterExpression);
                }
                catch (Exception e)
                {
                    // todo: Was sinnvolles machen - error möglichst schon vornerein abfangen und melden.
                    var error = e.ToString();
                }
            }
        }

        public FileRepresentation ChooseFile()
        {
            IEnumerable<FileRepresentation> filesToPickFrom;
            if (this.Settings.Prevalences.Any())
            {
                int maxValPrev = this.Settings.Prevalences.Sum(p => p.RelativePrevalence);
                int rolledValue = new Random().Next(maxValPrev);
                int current = 0;
                var targetFilter = this.Settings.Prevalences.SkipWhile(item =>
                {
                    current += item.RelativePrevalence;
                    return !(current > rolledValue);
                }).First();

                try
                {
                    filesToPickFrom = this.Data.ApplyFiltering(targetFilter.FilterExpression);
                }
                catch
                {
                    filesToPickFrom = this.Data;
                }
            }
            else
            {
                filesToPickFrom = this.Data;
            }

            if (filesToPickFrom.Count() == 0)
            {
                // todo: nach dem scan schon invalide zustände erkennen und melden
                throw new ApplicationException("There are empty pools or not files at all");
            }

            int targetIndex = new Random().Next(filesToPickFrom.Count());
            var targetFile = filesToPickFrom.ElementAt(targetIndex);
            return targetFile;
        }




    }
}
