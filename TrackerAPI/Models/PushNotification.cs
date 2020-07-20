using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerAPI.Models
{
    public class PushNotification
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
