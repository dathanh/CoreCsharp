using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Framework.Utility
{
    public static class CaculatorHelper
    {


        public static string GetContentFromXlmData(string key, int? value)
        {
            var xmlDataHelpper = AppDependencyResolver.Current.GetService<IXmlDataHelpper>();
            if (value.GetValueOrDefault() == 0 || xmlDataHelpper == null)
            {
                return "";
            }
            return xmlDataHelpper.GetValue(key, value.GetValueOrDefault().ToString());
        }

        public static int CaculateDuration(DateTime startDate, DateTime dueDate, DateTime? cancelDate, DateTime? completedDate)
        {
            if (cancelDate == null && completedDate == null)
            {
                if (DateTime.Now < dueDate)
                {
                    return (int)(DateTime.Now - startDate).TotalHours;
                }
                return (int)(DateTime.Now - dueDate).TotalHours;
            }

            if (cancelDate != null)
            {
                return (int)(((DateTime)cancelDate) - startDate).TotalHours;
            }
            return (int)(((DateTime)completedDate) - startDate).TotalHours;
        }

        public static string CaculateFormatDuration(int duration)
        {
            if (duration < 0)
            {
                return "0d0h";
            }
            return (duration / 24).ToString(CultureInfo.InvariantCulture) + "d" + (duration % 24).ToString(CultureInfo.InvariantCulture) + "h";
        }

        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, bool saturdayIsWeekend, List<DateTime> bankHolidays = null)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
            {
                throw new ArgumentException("Incorrect last day " + lastDay);
            }

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                {
                    lastDayOfWeek += 7;
                }

                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                    {
                        businessDays -= 2;
                    }
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                    {
                        businessDays -= 1;
                    }
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                {
                    businessDays -= 1;
                }
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            if (bankHolidays != null)
            {
                foreach (DateTime bankHoliday in bankHolidays)
                {
                    DateTime bh = bankHoliday.Date;
                    if (firstDay <= bh && bh <= lastDay)
                    {
                        --businessDays;
                    }
                }
            }

            if (!saturdayIsWeekend)
            {
                // Get number of saturday of month
                var timeSpanDateRange = lastDay - firstDay;                       // Total duration
                var countSaturday = (int)Math.Floor(timeSpanDateRange.TotalDays / 7);   // Number of whole weeks
                var remainder = (int)(timeSpanDateRange.TotalDays % 7);         // Number of remaining days
                var sinceLastDay = lastDay.DayOfWeek - DayOfWeek.Saturday;   // Number of days since last [day]
                if (sinceLastDay < 0)
                {
                    sinceLastDay += 7;         // Adjust for negative days since last [day]
                }

                // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
                if (remainder >= sinceLastDay)
                {
                    countSaturday++;
                }
                businessDays += countSaturday;
            }
            return businessDays;
        }

        /// <summary>
        /// Minimum: 8, at least 1 number, at least one upper case and at least one special character.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string password)
        {
            var pattern = "^(.{0,7}|[^0-9]*|[^A-Z]*|[^a-z]*|[a-zA-Z0-9]*)$";
            Regex regex = new Regex(pattern);
            return !regex.IsMatch(password);
        }

        public static bool IsValidPasswordCustomer(string password)
        {
            return password.Length >= 8;
            //var pattern = "^(.{0,7}|[^0-9]*|[^A-Z]*|[^a-z]*|[a-zA-Z0-9]*)$";
            //Regex regex = new Regex(pattern);
            //return !regex.IsMatch(password);
        }

        private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                {
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
                }
            }
            return str;
        }

        public static string FormatDataTimeToString(this DateTime date, string format)
        {
            if (date == DateTime.MinValue)
            {
                return "";
            }
            return date.ToString(format);
        }

        public static string GetUrlViaName(this string name)
        {
            name = name.RemoveSign4VietnameseString();
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            name = name.ToLower().Replace(" ", "-");
            name = name.Replace("*", "-");
            name = name.Replace("/", "-");
            name = name.Replace(@"\", "-");
            name = name.Replace("?", "-");
            name = name.Replace("&", "-");
            return name;
        }
    }
}
