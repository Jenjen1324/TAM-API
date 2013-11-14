using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace TAMClient.Connection
{
    public class TimeTable
    {
        public string name { get; set; }
        public List<DateTime> days = new List<DateTime>();
        public List<Row> rows { get; private set; }
		public string html { get; set; }

		public string GetHtmlTimeTable()
		{
			WebClient wc = new WebClient ();
			string style = wc.DownloadString ("https://info.tam.ch/data/kho/css/timetable.css");
			string data = "<style>" + style + "</style>";
			string html = this.html.Replace ("\n", "");
			MatchCollection m = Regex.Matches (html, "\\<table.+\\>(.*?)\\<\\/table\\>");
			return data + m [0].Groups [0].Captures [0].ToString ();
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
