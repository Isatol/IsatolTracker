using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrackerXamarinDemo.Database
{
    public class Database
    {
        private SQLiteAsyncConnection Connection;
        public Database()
        {
            Connection = DependencyService.Get<ISQLiteDB>().GetConnection();            
            Connection.CreateTableAsync<Database.Package>();
            Connection.CreateTableAsync<Database.LastPackageUpdate>();
            Connection.CreateTableAsync<Database.Minutes>();
        }
        
        public async Task<List<Package>> GetPackages()
        {
            var package = await Connection.Table<Package>().ToListAsync();
            return package;
        }

        public async Task<Package> GetPackage(string trackingNumber)
        {
            var package = await Connection.Table<Package>().FirstOrDefaultAsync(c => c.TrackingNumber == trackingNumber);
            return package;
        }

        public async Task<List<LastPackageUpdate>> GetLastPackageUpdates()
        {
            return await Connection.Table<LastPackageUpdate>().ToListAsync();
        }

        public async Task<LastPackageUpdate> GetLastPackageUpdate(int packageID)
        {
            return await Connection.Table<LastPackageUpdate>().FirstOrDefaultAsync(p => p.PackageID == packageID);
        }

        public async Task UpdateLastPackageUpdate(LastPackageUpdate lastPackageUpdate)
        {
            await Connection.UpdateAsync(lastPackageUpdate, typeof(LastPackageUpdate));
        }

        public async Task DeleteAllPackages()
        {
            await Connection.DeleteAllAsync<LastPackageUpdate>();
            await Connection.DeleteAllAsync<Package>();              
        }

        public async Task DeletePackage(int packagePrimaryKey)
        {
           var lastPackageUpdate = await GetLastPackageUpdate(packagePrimaryKey);
            if(lastPackageUpdate != null)
            {
                await Connection.DeleteAsync<LastPackageUpdate>(lastPackageUpdate.LastPackageUpdateID);
            }
            await Connection.DeleteAsync<Package>(packagePrimaryKey);
        }

        /// <summary>
        /// Returns the identity ID
        /// </summary>
        /// <returns></returns>
        public async Task<int> InsertPackage(Package package)
        {
            return await Connection.InsertAsync(package);
        }

        public async Task<int> InsertLastPackageUpdate(LastPackageUpdate lastPackageUpdate)
        {
            return await Connection.InsertAsync(lastPackageUpdate);
        }

        public async Task UpdateMinutes(Minutes minutes)
        {
            await Connection.UpdateAsync(minutes, typeof(Minutes));
        }

        public async Task InsertMinutes(Minutes minutes)
        {
            await Connection.InsertAsync(minutes);
        }

        public async Task<Minutes> GetMinutes()
        {
            return await Connection.Table<Minutes>().FirstOrDefaultAsync();
        }


        [Table("Package")]
        public class Package
        {
            [PrimaryKey, AutoIncrement]
            public int PackageID { get; set; }
            public string TrackingNumber { get; set; }
            public string Name { get; set; }
            public int CompanyID { get; set; }
            [OneToMany]
            public List<LastPackageUpdate> LastPackageUpdates { get; set; }
        }

        [Table("LastPackageUpdate")]
        public class LastPackageUpdate
        {
            [PrimaryKey, AutoIncrement]
            public int LastPackageUpdateID { get; set; }
            public DateTime? Date { get; set; }
            public string Event { get; set; }
            public string Messages { get; set; }
            [ForeignKey(typeof(Package))]
            public int PackageID { get; set; }
        }

        [Table("Minutes")]
        public class Minutes
        {
            [PrimaryKey, AutoIncrement]
            public int MinutesID { get; set; }
            public DateTime DateTime { get; set; }
        }
    }
}
    