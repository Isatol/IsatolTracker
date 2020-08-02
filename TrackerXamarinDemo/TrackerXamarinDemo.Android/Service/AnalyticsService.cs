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
using Firebase.Analytics;
using Plugin.CurrentActivity;
using TrackerXamarinDemo.Services;

namespace TrackerXamarinDemo.Droid.Service
{
    public class AnalyticsService : IAnalyticsService
    {

		public void LogEvent(string eventId)
		{
			LogEvent(eventId, null);
		}

		public void LogEvent(string eventId, string paramName, string value)
		{
				LogEvent(eventId, new Dictionary<string, string>
				{
					{paramName, value}
				});
		}

		public void SetUserId(string userId)
		{
			var fireBaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current.AppContext);

			fireBaseAnalytics.SetUserId(userId);
		}

		public void LogEvent(string eventId, IDictionary<string, string> parameters)
		{
			var fireBaseAnalytics = FirebaseAnalytics.GetInstance(CrossCurrentActivity.Current.AppContext);

			if (parameters == null)
			{
				fireBaseAnalytics.LogEvent(eventId, null);
				return;
			}

			var bundle = new Bundle();

			foreach (var item in parameters)
			{
				bundle.PutString(item.Key, item.Value);
			}

			fireBaseAnalytics.LogEvent(eventId, bundle);
		}
	}
}