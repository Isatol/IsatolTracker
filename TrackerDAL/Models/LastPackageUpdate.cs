using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerDAL.Models
{
    public class LastPackageUpdate
    {
        public int LastPackageUpdateID { get; set; }
        public DateTime? Date { get; set; }
        public string Event { get; set; }
        public int PackageID { get; set; }
    }
}
