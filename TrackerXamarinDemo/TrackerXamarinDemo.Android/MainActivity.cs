using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Matcha.BackgroundService.Droid;
using Android.App.Job;
using Android.Content;
using Plugin.LocalNotifications;
using Plugin.CurrentActivity;

namespace TrackerXamarinDemo.Droid
{
    [Activity(Label = "Isatol Package Tracker", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        JobScheduler job;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //BackgroundAggregator.Init(this);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.package_app_icon;
            UserDialogs.Init(this);
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            job = (JobScheduler)GetSystemService(JobSchedulerService);
            var javaClass = Java.Lang.Class.FromType(typeof(CheckPackagesJob));
            var compName = new ComponentName(this, javaClass);
            JobInfo jobInfo;            
                jobInfo = new JobInfo.Builder(1, compName)
                                     .SetRequiredNetworkType(NetworkType.Any)
                                     .SetPersisted(true)
                                     .SetPeriodic(600000)
                                     .SetRequiresBatteryNotLow(true)
                                     .SetBackoffCriteria(30000, BackoffPolicy.Linear)
                                     .Build();            
            var result = job.Schedule(jobInfo);
            if(result == JobScheduler.ResultSuccess)
            {
                Android.Util.Log.Info("myapp", "Tarea se ejecutó");
            }
            else
            {
                Android.Util.Log.Error("myapp", "Error en tarea");
            }
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}