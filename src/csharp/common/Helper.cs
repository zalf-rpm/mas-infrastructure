using System;

namespace Mas.Infrastructure.Common
{
    public class Helper
    {
        public static DateTime CommonDate2DateTime(Mas.Schema.Common.Date d)
        {
            return new DateTime(
                d.Year < 100 ? d.Year + 1 : d.Year, 
                d.Month == 0 ? 1 : d.Month, 
                d.Day == 0 ? 1 : d.Day
            );
        }

        public static string CommonDate2IsoDateString(Mas.Schema.Common.Date d)
        {
            return $"{d.Year:D4}-{d.Month:D2}-{d.Day:D2}";
        }

        public static Mas.Schema.Common.Date DateTime2CommonDate(DateTime d)
        {
            return new Mas.Schema.Common.Date { 
                Year = (short) (d.Year <= 100 ? d.Year - 1 : d.Year), 
                Month = (byte)d.Month, 
                Day = (byte)d.Day };
        }

        public static string Capitalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length == 1 ? char.ToUpper(s[0]).ToString() : char.ToUpper(s[0]) + s[1..];
        }

        public static string ToUpper(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            var res = new System.Text.StringBuilder();
            foreach (var c in s) res.Append(char.ToUpper(c));
            return res.ToString();
        }
    }
}
