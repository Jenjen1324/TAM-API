using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TAMClient.TAM_API;
using Android.Webkit;
using System.Threading.Tasks;

namespace TAMClient
{
	public class TimeTableHandler : MainActivity
	{


		/*public void TimeTableHandler(TamLogin session, Android.Content.Context context, Button btn_next, Button btn_prev, WebView web_view, ProgressBar progB, TextView textBox)
		{

		}

		private void Login()
		{
			progB.Indeterminate = true;
			Task.Factory.StartNew (delegate {
				session.Login ();
				RunOnUiThread(delegate { 
					LoadPage();
				});
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
				TimeTable tt = session.GetTimeTable (week);
				string html = tt.GetHtmlTimeTable();
				Util.IO.CacheData(context,"timetable_" + currentWeek,html);
				RunOnUiThread (delegate {
					LoadWebView(html);
					progB.Indeterminate = false;
				});
			});
		}

		private void LoadCache()
		{
			try
			{
				string html = Util.IO.LoadCachedData<string>(context, "timetable_" + currentWeek);
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
		}*/
	}
}

