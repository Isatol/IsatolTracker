using System;
using System.Collections.Generic;
using System.Text;

namespace Isatol.Tracker.Models
{
    public class UPS
    {
        public string Requester { get; set; } = "wt/trackdetails";
        public List<string> TrackingNumber { get; set; }

    }
}
