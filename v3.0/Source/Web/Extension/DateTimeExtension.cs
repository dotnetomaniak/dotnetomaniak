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
    }
}