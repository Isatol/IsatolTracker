using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerDAL
{
    public class TrackerSystem
    {
        private string _cs;
        public TrackerSystem(string cs)
        {
            _cs = cs;
        }

        public async Task<Models.Users> Login(string email, string password) 
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Email", email);
                parameters.Add("Password", password);
                return await connection.QuerySingleOrDefaultAsync<Models.Users>("System.Login", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task InsertNotification(Models.Notification notification)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Endpoint", notification.Endpoint);
                parameters.Add("Keys", notification.Keys);
                parameters.Add("UsersID", notification.UsersID);
                await connection.ExecuteAsync("System.InsertNotification", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<List<Models.Notification>> GetNotifications()
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                var enumerableNotifications = await connection.QueryAsync<Models.Notification>("System.GetNotifications", commandType: System.Data.CommandType.StoredProcedure);
                return enumerableNotifications.ToList();
            }
        }
        
        public async Task<List<Models.Users>> GetUsers()
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                var users = await connection.QueryAsync<Models.Users>("System.GetUsers", commandType: System.Data.CommandType.StoredProcedure);
                return users.ToList();
            }
        }

        public async Task<List<Models.Notification>> GetUserNotifications(int userID)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserID", userID);
                var notifications = await connection.QueryAsync<Models.Notification>("System.GetUserSubscription", parameters, commandType: System.Data.CommandType.StoredProcedure);
                return notifications.ToList();
            }
        }

        public async Task<Models.Users> GetUser(int userID)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserID", userID);
                return await connection.QuerySingleAsync<Models.Users>("System.GetUser", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task DeleteSuscription(int userID, string endpoint) 
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Endpoint", endpoint);
                parameters.Add("UserID", userID);
                await connection.ExecuteAsync("System.DeleteSuscription", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task AddLog(string log)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Log", log);
                await connection.ExecuteAsync("System.AddLog", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task UpdateReceiveEmails(int userID, bool receiveEmails)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserID", userID);
                parameters.Add("ReceiveEmails", receiveEmails);
                await connection.ExecuteAsync("System.UpdateReceiveEmails", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task AddUser(Models.Users users)
        {
            using(SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Email", users.Email);
                parameters.Add("Name", users.Name);
                parameters.Add("Password", users.Password);
                await connection.ExecuteAsync("System.AddUser", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public async Task<Models.Users> GetUser(string email)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Email", email);
                return await connection.QuerySingleOrDefaultAsync<Models.Users>("System.GetUserByEmail", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
