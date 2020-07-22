using System;
using System.Collections.Generic;
using System.Text;

namespace Isatol.Tracker.Models
{
    public class UPSResponse
    {
        public string StatusCode { get; set; }
        public string StatusText { get; set; }
        public List<TrackDetails> TrackDetails { get; set; }
    }

    public class TrackDetails
    {
        public string PackageStatus { get; set; }
        public string PackageStatusType { get; set; }
        public string EstimatedArrival { get; set; }
        public string EstimatedDeparture { get; set; }
        public List<ShipmentProgressActivities> ShipmentProgressActivities { get; set; }
    }

    public class ShipmentProgressActivities
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string ActivityScan { get; set; }

    }
}
