using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerDAL.Models
{
    public class CompanyPackage : Company
    {
        public string PackageName { get; set; }
        public int PackageID { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime Date { get; set; }
        public string Event { get; set; }
    }
}
