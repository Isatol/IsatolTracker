using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Tracker.Models.DHL
{
    public class DHLResponse
    {
        public List<Shipments> Shipments { get; set; }
    }

    public class Shipments
    {
        public string ID { get; set; }
        public Origin Origin { get; set; }
        public Destination Destination { get; set; }
        public CurrentStatus Status { get; set; }
        public List<Events> Events { get; set; }
    }

    public class Origin
    {
        public Address Address { get; set; }
    }
    public class Destination
    {
        public Address Address { get; set; }
    }
    
    public class Address
    {
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string AddressLocality { get; set; }
    }

    public class CurrentStatus
    {
        public string Timestamp { get; set; }
        public Location Location { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
    public class Location
    {
        public Address Address { get; set; }
    }

    public class Events
    {
        public string Timestamp { get; set; }
        public Location Location { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
    }
}
