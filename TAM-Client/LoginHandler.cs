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

namespace TAMClient
{
	public class LoginHandler : MainActivity
	{
		public LoginHandler()
		{
			Button btn = FindViewById<Button> (Resource.Id.button_login);
			btn.Click += HandleClick;
		}

		void HandleClick (object sender, EventArgs e)
		{
			throw new NotImplementedException ();
			string[] data;
			Util.IO.SaveDataArray ("cred.txt", data);
		}

	}
}

