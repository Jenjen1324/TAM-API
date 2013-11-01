using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAM_Client.Connection;

namespace TAM_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            WebTest();
        }

        void WebTest()
        {
            TamLogin request = new TamLogin()
            {
                username = "jens.vogler",
                password = "y_98diH[am",
                school = "kho",
                _class = "32"
            };

            request.Login();

            request.Test();

            /*
            TimeTable table = request.GetTimeTable();
            MessageBox.Show(table.name);*/
        }

        public void SetSource(string src)
        {
            webBrowser1.Navigate("about:blank");
            webBrowser1.Document.Write(src);
        }
    }
}
