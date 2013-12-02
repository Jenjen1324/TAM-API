using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TAMClient.Util;

namespace TAMClient.TAM_API
{
    public class TamLogin
    {
        public string username { get; set; }
        public string password
        {
            get
            {
                return password_enc;
            }
            set
            {
                password_enc = value;
            }
        }
        private string password_enc { get; set; }
        public string school { get; set; }
        public string _class { get; set; }

        private CookieContainer cookies { get; set; }

        public void Login()
        {
			if (username == null || password == null || school == null) {
				if (!LoadCred ())
					throw new Exception("No Login data provided");
			}

            Console.WriteLine("Logging in...");
            this.cookies = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://info.tam.ch/");

            string postdata = String.Format("school={0}&username={1}&password={2}",school,username,password);
            byte[] data = new ASCIIEncoding().GetBytes(postdata);

            request.CookieContainer = this.cookies;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            HttpWebResponse form = (HttpWebResponse)request.GetResponse();
        }

        /*public void Test()
        {
            Console.WriteLine("Testing");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://info.tam.ch/ajax/register");

            string postdata = String.Format("classbookId={0}&week={1}", "FSJA8mpf-hcLOoVQVnTLhHnOFVvTiq9_hh0W3PUl4thjPF41HBZbZhXlxbCE5zUHclJqornD0H_xZAFrDFAMIg..", "201339");
            byte[] data = new ASCIIEncoding().GetBytes(postdata);

            request.CookieContainer = this.cookies;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            HttpWebResponse form = (HttpWebResponse)request.GetResponse();

            string formPage;
            using (StreamReader response = new StreamReader(form.GetResponseStream(), Encoding.UTF8))
            {
                formPage = response.ReadToEnd();
            }

            Console.WriteLine(formPage);
        }*/

		private bool LoadCred()
		{
			try
			{
				string[] data = IO.LoadDataArray("cred.txt");
				this.username = data[0];
				this.password_enc = data[1];
				this.school = data[2];
				return true;
			}
			catch
			{
				return false;
			}
		}

        public TimeTable GetTimeTable(int weekNumber)
        {
			if (_class == null) {
				if (!LoadClass())
					throw new Exception ("No class provided");
			}

            Console.WriteLine("Requesting Timetable...");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://info.tam.ch/main.php?action=tt_oneclassNew&table=&list=0");

			string postdata = String.Format("sc={0}&wk={1}", _class, weekNumber.ToString());
			Console.WriteLine (postdata);
            byte[] data = new ASCIIEncoding().GetBytes(postdata);

            request.CookieContainer = this.cookies;
            request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            HttpWebResponse form = (HttpWebResponse)request.GetResponse();

            string formPage;
            using (StreamReader response = new StreamReader(form.GetResponseStream(), Encoding.UTF8))
            {
                formPage = response.ReadToEnd();
            }

			return new TimeTable () {
				html = formPage
			};
        }

		private bool LoadClass ()
		{
			try {
				this._class = IO.LoadData("class.txt");
				return true;
			}
			catch {
				return false;
			}
		}
    }
}
