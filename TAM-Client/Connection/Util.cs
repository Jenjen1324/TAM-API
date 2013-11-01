using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM_Client.Connection
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
    }
}
