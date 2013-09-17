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
            MatchCollection classes = Regex.Matches(html, ".+onMouseOver\\=\\\"tooltip\\(\\'([a-zA-Z0-9\\&\\;\\s]+)\\'\\)\\;\\\"\\s*onMouseOut\\=\\\"kill\\(\\)\\;\\\"\\>\\<strong\\>([a-zA-Z0-9\\&\\#\\;]+)\\<\\/strong\\>([a-zA-Z0-9\\\"\\&\\;]+)(\\<\\/span\\>\\<span\\s*id\\=\\\"[a-z0-9]*\\\"\\s*class\\=\\\"spanright\\s*tt\\-entry\\\"\\>([0-9]+))?");


            int i = 0;

            foreach (Match m in classes)
            {
                if (i == 5)
                {
                    Console.Write("\n");
                    i = 0;
                }
                Console.Write(m.Groups[2].Value + "\t");
                i++;
            }

            return new TimeTable();
        }

        public static TimeTable ParseXml(string html)
        {
            TimeTable table = null;
            Row row = null;

            StringReader sr = new StringReader(html);
            XmlReader xml = XmlReader.Create(sr);

            while (xml.Read())
            {
                if(xml.NodeType == XmlNodeType.Element)
                {
                    if (xml.Name == "table")
                    {
                        table = new TimeTable();
                    }
                    else if (table != null)
                    {
                        if (xml.Name == "th" && xml.GetAttribute(0).ToString() == "lesson-col")
                        {
                            row = new Row() { isDays = true };
                            row.time = xml.ReadContentAsString();
                        }
                        if (row != null)
                        {
                            if (xml.Name == "th" && xml.GetAttribute(1).ToString() == "head-day")
                            {
                                row.classes.Add(new _Class() { name = xml.ReadContentAsString() });
                            }
                            else if (xml.Name == "th" && xml.GetAttribute(0).ToString() == "lesson")
                            {

                            }
                        }
                    }
                }
            }

            return table;
        }
    }

    public class Row
    {
        public string time;
        public List<_Class> classes;
        public bool isDays = false;
    }

    public class _Class
    {
        public string toolTip;
        public string room;
        public string name;
        public string teacher;
    }
}
