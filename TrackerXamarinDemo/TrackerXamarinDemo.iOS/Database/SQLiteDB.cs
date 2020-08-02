using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SQLite;
using TrackerXamarinDemo.Database;
using TrackerXamarinDemo.iOS.Database;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDB))]
namespace TrackerXamarinDemo.iOS.Database
{
    public class SQLiteDB : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var docPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(docPath, AppSettings.DatabaseName);
            return new SQLiteAsyncConnection(path);
        }
    }
}