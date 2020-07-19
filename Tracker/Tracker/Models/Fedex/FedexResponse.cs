using System;
using System.Collections.Generic;
using System.Text;

namespace Isatol.Tracker.Models
{
    public class FedexResponse
    {
        public TrackPackagesResponse TrackPackagesResponse { get; set; }
    }

    public class TrackPackagesResponse
    {
        public bool successful { get; set; }
        public List<PackageList> PackageList { get; set; }
    }

    public class PackageList
    {
        public string KeyStatus { get; set; }
        public string KeyStatusCD { get; set; }
        public string displayShipDateTime { get; set; }
        public string displayEstDeliveryDateTime { get; set; }
        public List<ScanEventList> ScanEventList { get; set; }
    }

    public class ScanEventList
    {
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Status { get; set; }
        public string StatusCD { get; set; }
        public string ScanLocation { get; set; }
        public bool IsDelivered { get; set; }
    }
}
