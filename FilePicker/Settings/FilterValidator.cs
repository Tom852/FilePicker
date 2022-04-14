using FilePicker.Scanner;
using Gridify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Settings
{
    public class FilterValidator
    {

        public (bool HasMainFilterError, bool HasPrevalenceFilterError) Validate(SettingsModel settings)
        {
            bool mainFilterError = false;
            bool prevFilterError = false;

            foreach (var mainFilter in settings.MainFilters)
            {
                var builder = new QueryBuilder<FileRepresentation>()
                 .AddCondition(mainFilter.FilterExpression);

                mainFilterError |= !builder.IsValid();
            }

            foreach (var prevFilter in settings.Prevalences)
            {
                var builder = new QueryBuilder<FileRepresentation>()
                 .AddCondition(prevFilter.FilterExpression);

                prevFilterError |= !builder.IsValid();
            }

            return (mainFilterError, prevFilterError);
        }

        public bool IsValid(string filterExpression)
        {
            var builder = new QueryBuilder<FileRepresentation>()
                 .AddCondition(filterExpression);

            // todo: single words, die aber gültige FilterExpr sind wie eta "FullPath" werden nicht als fhelerhaft erkannt. ijwas all(c => char.isLetter(c)) checken.
            return builder.IsValid();
        }
    }
}
