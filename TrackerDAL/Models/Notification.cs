using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerDAL.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Endpoint { get; set; }
        public string Keys { get; set; }
        public int UsersID { get; set; }
    }
}
