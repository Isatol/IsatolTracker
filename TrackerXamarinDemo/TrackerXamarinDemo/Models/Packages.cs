using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TrackerXamarinDemo.Models
{
    public class PackageModel
    {
        public List<Package> Packages { get; set; }
    }

    public class Package
    {
        public string PackageName { get; set; }
        public int PackageID { get; set; }
        public string TrackingNumber { get; set; }
        public string Logo { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Event { get; set; }
        public ImageSource ImageSource { get; set; }
        public Isatol.Tracker.Models.TrackingModel TrackingModel { get; set; }        
    }
}


