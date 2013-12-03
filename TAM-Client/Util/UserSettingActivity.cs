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
using Android.Preferences;

namespace TAMClient
{
	public class UserSettingActivity : PreferenceActivity
	{
		public void onCreate(Bundle bundle)
		{
			base.OnCreate (bundle);

			//AddPreferencesFromResource (Resource.Layout.PreferenceScreen);
		}
	}
}

