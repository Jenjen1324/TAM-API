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
using Android.Webkit;

namespace TAMClient
{
	public class CustomWebViewClient : WebViewClient
	{
		public void onPageFinished(WebView view, string url)
		{
			Console.WriteLine ("FINISHED LOADING PAGE");
		}
	}
}

