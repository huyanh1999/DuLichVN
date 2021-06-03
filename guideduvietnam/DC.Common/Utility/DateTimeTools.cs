using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DC.Common.Utility
{
    public static class DateTimeTools
    {
        public static DateTime GetFirstDayOfMonth(int iMonth, int iYear)
        {
            DateTime dtFrom = new DateTime(iYear, iMonth, 1);
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

            return dtFrom;
        }



        public static DateTime GetFirstDayOfMonth(int iMonth)
        {
            DateTime dtFrom = new DateTime(DateTime.Now.Year, iMonth, 1);
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

            return dtFrom;
        }



        public static DateTime GetLastDayOfMonth(DateTime dtDate)
        {
            DateTime dtTo = dtDate;

            dtTo = dtTo.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));

            return dtTo;
        }



        public static DateTime GetFirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }



        public static DateTime GetLastDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
        {
            DateTime lastDayInWeek = dayInWeek.Date;
            while (lastDayInWeek.DayOfWeek != firstDay)
                lastDayInWeek = lastDayInWeek.AddDays(1);

            return lastDayInWeek;
        }



        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }
        

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }



        public static DateTime ConvertToDatetime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.Now;
            else
            {
                string[] arr = value.Split('/');
                string temp = string.Format("{0}-{1}-{2}", arr[2], arr[1], arr[0]);
                if (arr.Length != 3)
                    return DateTime.Now;
                else
                {
                    try
                    {
                        return DateTime.Parse(temp);
                    }
                    catch
                    {
                        return DateTime.Now;
                    }
                }
            }
        }


        public static DateTime? CFToDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            else
            {
                string[] arr = value.Split('/');
                if (arr.Length != 3)
                    return null;
                else
                {
                    string temp = string.Format("{0}-{1}-{2}", arr[2], arr[1], arr[0]);

                    try
                    {
                        return DateTime.Parse(temp);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }
    }
}