using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerDAL.Models
{
    public class Package
    {
        public int PackageID { get; set; }
        public string TrackingNumber { get; set; }
        public string Name { get; set; }
        public int CompanyID { get; set; }
        public int UsersID { get; set; }
    }
}
