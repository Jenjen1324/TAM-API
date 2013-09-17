using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAM_Client.Connection
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

        public TimeTable GetTimeTable()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://info.tam.ch/main.php?action=tt_oneclassNew&table=&list=0");

            string postdata = String.Format("sc={0}&wk=43", _class);
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

            return TimeTable.Parse(formPage);
        }
    }
}
