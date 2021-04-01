using System;

namespace Mas.Infrastructure.Common
{
    public class Helper
    {
        public static DateTime CommonDate2DateTime(Mas.Common.Date d)
        {
            return new DateTime(
                d.Year < 100 ? d.Year + 1 : d.Year, 
                d.Month == 0 ? 1 : d.Month, 
                d.Day == 0 ? 1 : d.Day
            );
        }

        public static string CommonDate2IsoDateString(Mas.Common.Date d)
        {
            return $"{d.Year:D4}-{d.Month:D2}-{d.Day:D2}";
        }

        public static Mas.Common.Date DateTime2CommonDate(DateTime d)
        {
            return new Mas.Common.Date { 
                Year = (short) (d.Year <= 100 ? d.Year - 1 : d.Year), 
                Month = (byte)d.Month, 
                Day = (byte)d.Day };
        }

    }
}
