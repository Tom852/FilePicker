using FilePicker.Settings;
using Gridify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Scanner
{
    public class FileCounter
    {
        private FilterValidator Validator { get; } = new FilterValidator();
        public FileCountResult CountFiles(IQueryable<FileRepresentation> data, SettingsModel settings)
        {

            var validatorResult = Validator.Validate(settings);
            if (validatorResult.HasMainFilterError || validatorResult.HasPrevalenceFilterError)
            {
                return null;
            }

            var result = new FileCountResult();


            // apply prefilters geschlossen
            IQueryable<FileRepresentation> prefilteredData;
            var builder = new QueryBuilder<FileRepresentation>();
            settings.MainFilters
                .Select(f => f.FilterExpression)
                .Where(expr => !string.IsNullOrWhiteSpace(expr))
                .ToList()
                .ForEach(expr => builder.AddCondition(expr));
            prefilteredData = builder.Build(data);
            result.FileCountBeforeMainFilters = data.Count();
            result.FileCountAfterMainFilters = prefilteredData.Count();





            // count each prefilter
            foreach (var item in settings.MainFilters)
            {
                var count = data.ApplyFiltering(item.FilterExpression).Count();
                result.FileCountsForEachMainFilter.Add(count);
            }


            // count prevalences
            foreach (var item in settings.Prevalences)
            {
                var count = prefilteredData.ApplyFiltering(item.FilterExpression).Count();
                result.FileCountsForEachPrevalencePool.Add(count);
            }


            return result;
        }
    }
}
