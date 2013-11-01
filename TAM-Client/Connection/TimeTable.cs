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
        public List<DateTime> days = new List<DateTime>();
        public List<Row> rows { get; private set; }


        public static TimeTable Parse(string html)
        {
            Match days = Regex.Match(html, "class\\=\\\"head-day\\\"\\>(\\w+)");
            MatchCollection classes = Regex.Matches(html, 
                ".+class\\=\\\"empty\\\"\\s*\\>([a-zA-Z&;]*)|.*class\\=\\\"([\\w\\s]*)\\\".+onmouseover\\=\\\"tooltip\\(\\'([a-zA-Z0-9\\&\\;\\s]+)\\'\\)\\;\\\"\\s*onmouseout\\=\\\"kill\\(\\)\\;\\\"\\>\\<strong\\>([a-zA-Z0-9\\&\\#\\;]+)\\<\\/strong\\>([a-zA-Z0-9\\\"\\&\\;]+)(\\<\\/span\\>\\<span\\s*id\\=\\\"[a-z0-9]*\\\"\\s*class\\=\\\"spanright\\s*tt\\-entry\\\"\\>([0-9]+))?"
                ,RegexOptions.IgnoreCase);


            int i = 0;

            foreach (Match m in classes)
            {
                if (i == 5)
                {
                    Console.Write("\n");
                    i = 0;
                }
                Console.Write(m.Groups[4].Value + "\t");
                i++;
            }

            return new TimeTable();
        }

        public static TimeTable ParseXml(string html)
        {
            TimeTable table = null;

            // To prevent xml errors
            html = html.Replace("\" >", "\" />");

            StringReader sr = new StringReader(html);
            XmlReader xml = XmlReader.Create(sr,new XmlReaderSettings() {DtdProcessing = DtdProcessing.Ignore });
            //xml.Settings.DtdProcessing = DtdProcessing.Ignore;
            Console.WriteLine("Parsing XML");

            while (xml.Read())
            {
                if (xml.NodeType == XmlNodeType.Element)
                {
                    // Detect when the timetable starts
                    if (xml.Name == "table" && xml.GetAttribute("id") == "timetable")
                    {
                        table = new TimeTable();
                        XmlReader table_xml = xml.ReadSubtree();

                        while (table_xml.Read())
                        {
                            /// Read the dates
                            #region
                            if (table_xml.NodeType == XmlNodeType.Element 
                                && table_xml.Name == "tr" 
                                && table_xml.AttributeCount == 0)
                            {
                                List<DateTime> days = new List<DateTime>();
                                XmlReader date_xml = table_xml.ReadSubtree();
                                while (date_xml.Read())
                                {
                                    if (date_xml.GetAttribute("class") == "head-day")
                                    {
                                        string[] val = table_xml.ReadContentAsString().Split('>');
                                        string[] values = val[1].Split('.');
                                        days.Add(
                                            new DateTime(
                                                Convert.ToInt16(values[2]),
                                                Convert.ToInt16(values[1]),
                                                Convert.ToInt16(values[0])
                                                )
                                            );
                                    }
                                }

                                table.days = days;
                            }
                            #endregion

                            if (table_xml.NodeType == XmlNodeType.Element 
                                && table_xml.Name == "tr" 
                                && table_xml.AttributeCount == 1)
                            {
                                Row row = new Row();
                                XmlReader row_xml = table_xml.ReadSubtree();
                                while (row_xml.Read())
                                {
                                    if (row_xml.NodeType == XmlNodeType.Element 
                                        && row_xml.Name == "th" 
                                        && row_xml.GetAttribute("class") == "lesson")
                                    {
                                        row.time = Util.ReplaceHtmlTags(row_xml.ReadContentAsString());
                                    }
                                    else if (row_xml.NodeType == XmlNodeType.Element 
                                        && row_xml.Name == "td" 
                                        && row_xml.GetAttribute("class") == "middle")
                                    {
                                        _Class _c = new _Class();
                                        XmlReader class_xml = row_xml.ReadSubtree();
                                        while (class_xml.Read())
                                        {
                                            if (class_xml.NodeType == XmlNodeType.Element
                                                && class_xml.Name == "span"
                                                && class_xml.GetAttribute("class") == "spanleft")
                                            {

                                                _c.toolTip = xml.GetAttribute("onMouseOver").Replace("tooltip('", "").Replace("');", "");

                                                string val = class_xml.ReadContentAsString();
                                                val = val.Replace("<strong>", "").Replace("</strong>", "").Replace("&nbsp",";");
                                                string[] values = val.Split(';');
                                                _c.name = values[0];
                                                _c.teacher = values[1];
                                            }

                                            if (class_xml.NodeType == XmlNodeType.Element
                                                && class_xml.Name == "span"
                                                && class_xml.GetAttribute("class").Contains("spanright"))
                                            {
                                                _c.room = class_xml.ReadContentAsString().Replace("<br />", "");
                                            }
                                        }
                                        row.classes.Add(_c);
                                    }
                                }

                                table.rows.Add(row);
                            }
                        }
                    }
                }

                        /*
                    else if (table != null)
                    {
                        if (isEditingClass)
                        {
                            if (xml.Name == "spanleft")
                            {
                                _class.toolTip = xml.GetAttribute("onMouseOver").Replace("tooltip('", "").Replace("');","");
                            }
                        } else
                        // Detect when the dates start
                        if (xml.Name == "th" && xml.GetAttribute(0).ToString() == "lesson-col")
                        {
                            days = new List<DateTime>();
                        }
                        else if (days != null)
                        {
                            if (xml.Name == "th" && xml.GetAttribute("class") == "head-day")
                            {
                                string[] val = xml.ReadContentAsString().Split('>');
                                string[] values = val[1].Split('.');
                                days.Add(
                                    new DateTime(
                                        Convert.ToInt16(values[2]),
                                        Convert.ToInt16(values[1]),
                                        Convert.ToInt16(values[0])
                                        )
                                    );
                            }
                        }
                        else if (xml.Name == "th" && xml.GetAttribute("class") == "lesson")
                        {
                            row = new Row();
                            row.time = Util.ReplaceHtmlTags(xml.ReadContentAsString());
                        }
                        if (row != null)
                        {
                            if (xml.Name == "td")
                            {
                                if (xml.GetAttribute("class") == "middle")
                                {
                                    _class = new _Class();
                                    isEditingClass = true;
                                    
                                }
                                else if (xml.GetAttribute("class") == "empty")
                                {
                                    row.classes.Add(new _Class() { empty = true });
                                }
                            }
                        }
                    }
                } else if (xml.NodeType == XmlNodeType.EndElement)
                {

                }*/
            }

            return table;
        }
    }

    public class Row
    {
        public string time;
        public List<_Class> classes;
    }

    public class _Class
    {
        public bool empty;

        public string toolTip;
        public string room;
        public string name;
        public string teacher;
    }
}
