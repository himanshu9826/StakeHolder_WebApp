using System;
using System.ComponentModel;
using System.Reflection;

namespace StakeHolder.Common.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }

        /// <summary>
        /// Method to get the date for given day
        /// </summary>
        /// <param name="dayString">Day Of Week in string</param>
        /// <returns>DateTime</returns>
        public static DateTime GetNextDateForDay(string  dayString)
        {
            DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayString, true);
            DateTime result = DateTime.Now.AddDays(1);
            while (result.DayOfWeek != day)
                result = result.AddDays(1);
            return result;
        }

       
    }


}
