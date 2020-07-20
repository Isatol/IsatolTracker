using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerAPI.Models
{
    public class NotificationSubscription
    {
        public Lib.Net.Http.WebPush.PushSubscription PushSubscription { get; set; }
        public int UserID { get; set; }
    }
}
