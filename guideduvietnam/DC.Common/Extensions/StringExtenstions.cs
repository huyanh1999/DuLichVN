using System;
using System.Collections.Generic;
using DC.Common.Helpers;

namespace DC.Common.Extensions
{
    public static class StringExtenstions
    {

        public static string ToSeparationString(this List<string> source, char separation)
        {
            var result = string.Empty;

            foreach (var item in source)
            {

                result += string.Format("{0}{1}",separation, item);
            }

            return result.Trim(separation);

        }
        public static string ToTimeAgo(this DateTime source)
        {
            return StringHelpers.TimeAgo(source);

        }
    }
}
