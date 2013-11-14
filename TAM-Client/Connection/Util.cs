using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TAMClient.Connection
{
    public static class Util
    {
        public static string ReplaceHtmlTags(string text)
        {
            return text
                .Replace("&ndash;", "-")
                .Replace("&nbsp;", "")
                .Replace("&uuml;", "ü")
                .Replace("&auml;", "ä")
                .Replace("&ouml;", "ö");

        }

		public static int GetCurrentWeekNumber()
		{
			var currentCulture = CultureInfo.CurrentCulture;
			var weekNo = currentCulture.Calendar.GetWeekOfYear(
				DateTime.Now,
				currentCulture.DateTimeFormat.CalendarWeekRule,
				currentCulture.DateTimeFormat.FirstDayOfWeek);
			return weekNo;
		}
    }
}
