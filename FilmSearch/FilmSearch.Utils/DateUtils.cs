using System;
using System.Globalization;

namespace FilmSearch.Utils
{
    public static class DateUtils
    {
        public static readonly CultureInfo DefaultCulture = CultureInfo.InvariantCulture;
        public const string DefaultFormat = "d";

        public static DateTime ParseDate(string date)
        {
            return DateTime.ParseExact(date, DefaultFormat, DefaultCulture);
        }
        
        public static string ParseDate(DateTime date)
        {
            return date.ToString(DefaultFormat, DefaultCulture);
        }
    }
}