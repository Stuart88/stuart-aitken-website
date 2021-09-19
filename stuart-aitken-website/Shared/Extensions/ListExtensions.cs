using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SharedProject.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// For creating dropdown lists of string values with a default empty value. Sorted alphabetically.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<string> AppendEmptyAndSort(this IEnumerable<string> list, string emptyValue)
        {
            list = list.Append(emptyValue);

            return list.OrderByDescending(i => i == emptyValue)
                       .ThenBy(i => i)
                       .ToList();
        }
    }
}
