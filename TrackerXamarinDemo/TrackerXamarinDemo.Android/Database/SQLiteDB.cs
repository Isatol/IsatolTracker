using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using TrackerXamarinDemo.Database;
using TrackerXamarinDemo.Droid.Database;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDB))]
namespace TrackerXamarinDemo.Droid.Database
{
    public class SQLiteDB : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(docPath, AppSettings.DatabaseName);
            return new SQLiteAsyncConnection(path);
        }
    }
}