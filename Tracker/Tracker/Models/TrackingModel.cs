using System;
using System.Collections.Generic;
using System.Text;

namespace IsatolTracker.Models
{
    public class TrackingModel
    {
        public DateTime? EstimateDelivery { get; set; }
        public bool Delivered { get; set; }
        public string Status { get; set; }
        public List<TrackingDetails> TrackingDetails { get; set; }
    }

    public class TrackingDetails
    {
        public DateTime Date { get; set; }
        public string Event { get; set; }
        public string Messages { get; set; }
    }
}
