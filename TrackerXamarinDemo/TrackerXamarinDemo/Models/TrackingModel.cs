using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerXamarinDemo.Models
{
    public class Tracking
    {
        public TrackingStatus TrackingModel { get; set; }
    }

    public class TrackingStatus 
    {
        public DateTime? EstimateDelivery { get; set; }
        public bool Delivered { get; set; }
        public string Status { get; set; }
        public List<TrackingDetails> TrackingDetails { get; set; }
    }

    public class TrackingDetails
    {
        public DateTime? Date { get; set; }
        public string Event { get; set; }
        public string Messages { get; set; }
        public string StringDate { get; set; }
    }
}
