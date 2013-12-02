using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Android.Animation;
using Android.Preferences;
using TAMClient.TAM_API;
using Android.Net;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace TAMClient
{
	[Activity (Label = "TAM-Client", MainLauncher = true, Theme = "@android:style/Theme.Holo.Light.NoActionBar")]
	public class MainActivity : Activity
	{
		TamLogin session;

		#region TimeTableVar
		WebView web_view;
		ProgressBar progB;
		TextView textBox;
		int currentWeek;
		#endregion

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


			string pwd = "y_98diH[am";
			session = new TamLogin()
			{
				username = "jens.vogler",
				password = pwd,
				school = "kho",
				_class = "32"
			};
			//session = new TamLogin ();
			_HandleTimeTable ();
		}

		private void _HandleTimeTable()
		{
			var menu = FindViewById<FlyOutContainer> (Resource.Id.FlyOutContainer);
			var menuButton = FindViewById (Resource.Id.MenuButton);
			menuButton.Click += (sender, e) => {
				menu.AnimatedOpened = !menu.AnimatedOpened;
			};

			Button btn_next = FindViewById<Button> (Resource.Id.button_next);
			Button btn_prev = FindViewById<Button> (Resource.Id.button_prev);
			web_view = FindViewById<WebView> (Resource.Id.webView1);
			progB = FindViewById<ProgressBar> (Resource.Id.progressBar1);
			textBox = FindViewById<TextView> (Resource.Id.textView_week);

			web_view.Settings.JavaScriptEnabled = true;
			web_view.SetWebViewClient (new CustomWebViewClient());
			currentWeek = TAM_API.Util.GetCurrentWeekNumber (); 
			textBox.Text = "Woche: " + Convert.ToString (currentWeek);

			btn_prev.Click += delegate(object sender, EventArgs e) {
				currentWeek--;
				LoadCache();
				LoadPage ();
			};
			btn_next.Click += delegate(object sender, EventArgs e) {
				currentWeek++;
				LoadCache();
				LoadPage ();
			};

			LoadCache ();
			Login ();
		}

		private void _HandleLogin()
		{
			SetContentView (Resource.Layout.Login);

			Button btn = FindViewById<Button> (Resource.Id.button_login);
			btn.Click += delegate {
				session.username = FindViewById<TextView> (Resource.Id.editText_uname).Text;
				session.password = FindViewById<TextView> (Resource.Id.editText_pwd).Text;
				session.school = /*FindViewById<Spinner> (Resource.Id.spinner_school).SelectedItem;*/ "kho";
				RunOnUiThread(_HandleTimeTable);
			};
		}

		#region TimeTableMethods
		private void Login()
		{
			progB.Indeterminate = true;
			Task.Factory.StartNew (delegate {
				try
				{
					session.Login ();
					RunOnUiThread(delegate { 
						LoadPage();
					});
				} catch {
					RunOnUiThread(delegate { 
						_HandleLogin();
					});
				}
			});
		}

		private void LoadPage()
		{
			LoadPage (currentWeek);
		}

		private void LoadPage(int week)
		{
			Console.WriteLine ("Loading week: "+ week);
			textBox.Text = "Woche: " + Convert.ToString (week);
			progB.Indeterminate = true;
			Task.Factory.StartNew (delegate {
				TimeTable tt;
				try
				{
					tt = session.GetTimeTable (week);
				}
				catch (Exception e)
				{
					if(e.Message == "No class provided")
					{
						RunOnUiThread(HandleSelectClass);
						return;
					} else {
						throw e;
					}
				}
				string html = tt.GetHtmlTimeTable();
				Util.IO.CacheData(this,"timetable_" + currentWeek,html);
				RunOnUiThread (delegate {
					LoadWebView(html);
					progB.Indeterminate = false;
				});
			});
		}

		private void HandleSelectClass()
		{
			progB.Indeterminate = false;

		}

		private void LoadCache()
		{
			try
			{
				string html = Util.IO.LoadCachedData<string>(this, "timetable_" + currentWeek);
				LoadWebView(html);
			}
			catch
			{
				Console.WriteLine ("No cached data availible");
			}
		}

		private void LoadWebView(string data)
		{
			int x, y;
			x = web_view.ScrollX;
			y = web_view.ScrollY;
			web_view.LoadDataWithBaseURL (null,data, "text/html","utf-8", null);
			web_view.Download += delegate(object sender, DownloadEventArgs e) {
				RunOnUiThread (delegate {
					web_view.ScrollTo (x, y);
				});
			};
		}
		#endregion
	}
}


