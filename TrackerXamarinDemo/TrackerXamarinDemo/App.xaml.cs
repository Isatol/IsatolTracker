using Matcha.BackgroundService;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrackerXamarinDemo.Database;
using TrackerXamarinDemo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrackerXamarinDemo
{
    public partial class App : Application
    {
        public static SQLiteAsyncConnection Connection;
        public static Database.Database Database;
        public App()
        {                  
            InitializeComponent();
            Connection = DependencyService.Get<ISQLiteDB>().GetConnection();
            Database = new Database.Database();
            //Task.Run(async () =>
            //{
            //    //var packages = await Database.GetPackages();
            //    //if (packages.Count > 0) await Database.DeleteAllPackages();
            //    CrossLocalNotifications.Current.Show("Test", "Test notification");
            //    await Database.DeleteAllPackages();
            //});
            MainPage = new NavigationPage(new Views.Tracker());


            //Connection.QueryAsync<Models.User>("DELETE FROM User");
            //Models.User user = Connection.QueryAsync<Models.User>("SELECT * FROM User").Result.FirstOrDefault();

            //if(user != null)
            //{            
            //}            
            //else
            //{
            //    MainPage = new NavigationPage(new Login());
            //}
        }

        protected override void OnStart()
        {
            //BackgroundAggregatorService.Add(() => new Services.BackgroundTrackPackage(60, Database));
            //BackgroundAggregatorService.StartBackgroundService();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
