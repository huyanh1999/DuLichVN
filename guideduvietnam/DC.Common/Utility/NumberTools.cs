using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Common.Utility
{
    public static class NumberTools
    {
        #region #Specials
        public static decimal? ToDecimals(string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.ToNumericOnly();
            if (!string.IsNullOrEmpty(value))
            {
                decimal isDecial;
                if (decimal.TryParse(value, out isDecial))
                {
                    return decimal.Parse(value);
                }
            }

            return null;
        }
        #endregion

        // Array
        public static List<int?> ConvertToList(string values)
        {
            List<int?> items = new List<int?>();

            if (!string.IsNullOrWhiteSpace(values))
            {
                values = values.Replace("[", string.Empty).Replace("]", string.Empty);
                string[] arr = values.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    items.Add(ConvertToInt(arr[i]));
                }
            }

            return items;
        }

        public static List<int> ConvertToListInt(string values)
        {
            List<int> items = new List<int>();

            if (!string.IsNullOrWhiteSpace(values))
            {
                values = values.Replace("[", string.Empty).Replace("]", string.Empty);
                string[] arr = values.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    items.Add(ConvertToInt(arr[i]));
                }
            }

            return items;
        }

        public static int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }

        public static decimal ConvertToDecimal(string value)
        {
            decimal isDecimal;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(".", string.Empty).Replace(",", string.Empty);
                if (decimal.TryParse(value, out isDecimal))
                    return decimal.Parse(value);
            }

            return 0;
        }

        public static decimal ConvertToDecimal(decimal? decimalValue)
        {
            if (decimalValue != null)
                return decimalValue.Value;

            return 0;
        }

        public static double ConvertToDouble(decimal? decimalValue)
        {
            if (decimalValue != null)
                return Convert.ToDouble(decimalValue.Value);

            return 0;
        }

        public static int ConvertToInt(string value)
        {
            int isInt;
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace(".", string.Empty).Replace(",", string.Empty);
                if (int.TryParse(value, out isInt))
                    return int.Parse(value);
            }

            return 0;
        }

        public static int ConvertToInt(int? value)
        {
            if (value != null)
                return value.Value;

            return 0;
        }

        public static short ConvertToShort(short? value)
        {
            if (value != null)
                return value.Value;

            return 0;
        }



        public static short ConvertToShort(string value)
        {
            short isInt;
            if (!string.IsNullOrEmpty(value) && short.TryParse(value, out isInt))
            {
                return short.Parse(value);
            }

            return (short)0;
        }

    }
}