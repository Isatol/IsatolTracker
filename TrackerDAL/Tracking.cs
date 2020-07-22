using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerDAL.Models;

namespace TrackerDAL
{
    public class Tracking
    {
        private string _cs;
        public Tracking(string cs)
        {
            _cs = cs;
        }

        public async Task<List<Company>> GetCompanies()
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                var companies = await connection.QueryAsync<Models.Company>("Tracking.GetCompanies", commandType: System.Data.CommandType.StoredProcedure);
                return companies.ToList();
            }
        }
        
        public async Task<Models.Company> GetCompany(int companyID)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("CompanyID", companyID);
                return await connection.QuerySingleOrDefaultAsync<Company>("Tracking.GetCompany", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Models.LastPackageUpdate> GetLastPackageUpdate(int packageID)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("PackageID", packageID);
                return await connection.QuerySingleOrDefaultAsync<LastPackageUpdate>("Tracking.GetLastPackageUpdate", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task InsertLastPackageUpdate(Models.LastPackageUpdate lastPackageUpdate)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Date", lastPackageUpdate.Date, System.Data.DbType.DateTime);
                parameters.Add("Event", lastPackageUpdate.Event);
                parameters.Add("PackageID", lastPackageUpdate.PackageID);
                await connection.ExecuteAsync("Tracking.InsertLastPackageUpdate", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task UpdateLastPackage(LastPackageUpdate lastPackageUpdate)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Date", lastPackageUpdate.Date, System.Data.DbType.DateTime);
                parameters.Add("Event", lastPackageUpdate.Event);
                parameters.Add("PackageID", lastPackageUpdate.PackageID);
                await connection.ExecuteAsync("Tracking.UpdateLastPackage", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<List<Package>> GetPackages()
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                var packages = await connection.QueryAsync<Package>("Tracking.GetPackages", commandType: System.Data.CommandType.StoredProcedure);
                return packages.ToList();
            }
        }

        public async Task InsertPackage(Package package)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("CompanyID", package.CompanyID);
                parameters.Add("TrackingNumber", package.TrackingNumber);
                parameters.Add("UsersID", package.UsersID);
                parameters.Add("Name", package.Name);
                await connection.ExecuteAsync("Tracking.InsertPackage", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<List<CompanyPackage>> GetUserPackages(int userID)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserID", userID);
                var packages = await connection.QueryAsync<CompanyPackage>("System.GetUserPackages", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return packages.ToList();
            }
        }
    }
}
