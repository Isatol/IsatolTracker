using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerDAL.Models
{
    public class Users
    {
        public int UsersID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ReceiveEmails { get; set; }
    }
}
