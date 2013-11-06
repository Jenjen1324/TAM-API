using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using TAM_Client.Connection;
using Android.Net;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TAMClient
{
	[Activity (Label = "TAM-Client", MainLauncher = true)]
	public class MainActivity : Activity
	{
		WebView web_view;
		ProgressBar progB;
		TextView text;
		TamLogin session;

		/*bool isNetworkConnected { get {
				ConnectivityManager cm = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
				NetworkInfo ni = cm.getActiveNetworkInfo();
				if (ni == null) {
					// There are no active networks.
					return false;
				} else
					return true;
			}
		}*/

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			Button button = FindViewById<Button> (Resource.Id.button1);
			button.Click += button_click;

			web_view = FindViewById<WebView> (Resource.Id.webView1);
			web_view.Settings.JavaScriptEnabled = true;
			progB = FindViewById<ProgressBar> (Resource.Id.progressBar1);
			text = FindViewById<TextView> (Resource.Id.textView1);

			string pwd = "y_98diH[am";
			session = new TamLogin()
			{
				username = "jens.vogler",
				password = pwd,
				school = "kho",
				_class = "32"
			};

			Login ();

		}

		private void button_click(Object sender, EventArgs e)
		{
			LoadPage ();
		}

		private void Login()
		{
			text.Text = "Logging in...";
			progB.Visibility = ViewStates.Visible;
			Task.Factory.StartNew (delegate {
				session.Login ();
				RunOnUiThread(delegate { 
					text.Text = "";
					progB.Visibility = ViewStates.Invisible;
					LoadPage();
				});
			});
		}

		private void LoadPage()
		{
			progB.Visibility = ViewStates.Visible;
			Task.Factory.StartNew (delegate {
				TimeTable tt = session.GetTimeTable ();
				RunOnUiThread (delegate {
					web_view.LoadData (tt.GetHtmlTimeTable(), "text/html", null);
					text.Text = "";
					progB.Visibility = ViewStates.Invisible;
				});
			});
		}
	}
}


