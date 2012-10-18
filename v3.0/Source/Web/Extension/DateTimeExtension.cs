namespace Kigg.Web
{
    using System;
    using System.Text;

    public static class DateTimeExtension
    {
        public static string ToRelative(this DateTime target)
        {
            Check.Argument.IsNotInFuture(target, "target");

            StringBuilder result = new StringBuilder();
            TimeSpan diff = (SystemTime.Now() - target);

            Action<int, string> format = (v, u) =>
            {
                if (v > 0)
                {
                    if (result.Length > 0)
                    {
                        result.Append(", ");
                    }

                    if (u == "dzieñ" && v > 1)
                        u = "dni";

                    if (u == "godzinê" && v > 1)
                    {
                        if ((v % 10 >= 5) || (v % 10 <= 1) || (v > 4 && v < 22))
                        {
                            u = "godzin";
                        }
                        else
                        {
                            u = "godziny";
                        }
                    }

                    if (u == "minutê" && v > 1)
                    {
                        if ((v % 10 >= 5) || (v % 10 <= 1) || (v > 4 && v < 22))
                        {
                            u = "minut";
                        }
                        else
                        {
                            u = "minuty";
                        }
                    }

                    result.Append("{0} {1}".FormatWith(v, u));
                }
            };

            format(diff.Days, "dzieñ");
            format(diff.Hours, "godzinê");
            format(diff.Minutes, "minutê");


            return (result.Length == 0) ? "chwilê" : result.ToString();
        }



        public static DateTime GetLastWeekdayOfMonth(this DateTime date, DayOfWeek day)
        {
            DateTime lastDayOfMonth = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
            int wantedDay = (int)day;
            int lastDay = (int)lastDayOfMonth.DayOfWeek;
            return lastDayOfMonth.AddDays(lastDay >= wantedDay ? wantedDay - lastDay : wantedDay - lastDay - 7);
        }

        public static string GetLocalTimeName(this DateTime date, bool? isSummerTime = null)
        {
            if (isSummerTime == null)
                isSummerTime = IsSummerTime(date);

            return isSummerTime.Value ? "CEST" : "CET";
        }

        public static int GetHoursDifferenceForLocalTime(this DateTime date, bool? isSummerTime = null)
        {
            if (isSummerTime == null)
                isSummerTime = IsSummerTime(date);

            return isSummerTime.Value ? 1 : -1;
        }

        public static bool IsSummerTime(this DateTime date)
        {
            var lastSundayInMarch = GetLastWeekdayOfMonth(new DateTime(date.Year, 3, 1), DayOfWeek.Sunday);
            var lastSundayInOctober = GetLastWeekdayOfMonth(new DateTime(date.Year, 10, 1), DayOfWeek.Sunday);

            if ((date > lastSundayInMarch || date == lastSundayInMarch && date.Hour >= 2))
                if ((date < lastSundayInOctober || date == lastSundayInOctober && date.Hour <= 3))
                    return true;


            return false;
        }


    }
}
