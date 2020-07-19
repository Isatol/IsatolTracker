using System;
using System.Collections.Generic;
using System.Text;

namespace Isatol.Tracker.Models
{
    public class Fedex
    {        
        public TrackPackagesRequest TrackPackagesRequest { get; set; }     
    }

    public class TrackPackagesRequest
    {
        public string appType { get; set; }
        public string uniqueKey { get; set; }
        public List<TrackingInfoList> trackingInfoList { get; set; }
        public ProcessingParameters processingParameters { get; set; }
    }
    
    public class TrackingInfoList
    {
        public TrackNumberInfo trackNumberInfo { get; set; }
    }

    public class ProcessingParameters
    {
        //public bool anonymousTransaction { get; set; }
        //public string clientId { get; set; }
        //public bool returnDetailedErrors { get; set; }
        //public bool returnLocalizedDateTime { get; set; }
    }
    public class TrackNumberInfo
    {
        public string trackingNumber { get; set; }
        public string trackingQualifier { get; set; }
        public string trackingCarrier { get; set; }
    }
}
