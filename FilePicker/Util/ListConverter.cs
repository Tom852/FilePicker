using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Util
{
    public static class ListConverter
    {
        public static List<T> ConvertAndCastToGenericList<T>(this IList source)
        {
            List<T> result = new List<T>();
            foreach (var item in source)
            {
                result.Add((T)item);
            }
            return result;
        }
    }
}
