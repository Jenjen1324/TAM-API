using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace TAM_Client.Connection
{
    public class TimeTable
    {
        public string name { get; set; }
        public List<Row> rows { get; private set; }


        public static TimeTable Parse(string html)
        {
            Match days = Regex.Match(html, "class\\=\\\"head-day\\\"\\>(\\w+)");
            Match classes = Regex.Match(html, ".+onMouseOver\\=\\\"tooltip\\(\\'([a-zA-Z0-9\\&\\;\\s]+)\\'\\)\\;\\\"\\s*onMouseOut\\=\\\"kill\\(\\)\\;\\\"\\>\\<strong\\>([a-zA-Z0-9\\&\\#\\;]+)\\<\\/strong\\>([a-zA-Z0-9\\\"\\&\\;]+)(\\<\\/span\\>\\<span\\s*id\\=\\\"[a-z0-9]*\\\"\\s*class\\=\\\"spanright\\s*tt\\-entry\\\"\\>([0-9]+))?");

            return new TimeTable() {
            name = classes.Groups[0].Captures[1].Value.ToString()
            };
        }
    }

    public class Row
    {
        public List<_Class> classes;
    }

    public class _Class
    {
        string toolTip;
        string room;
        string name;
        string teacher;
    }
}
