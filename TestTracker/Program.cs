using System;
using System.Net.Http;

namespace TestTracker
{
    class Program
    {
        static void Main(string[] args)
        {            
            HttpClient httpClient = new HttpClient();
            Isatol.Tracker.Track track = new Isatol.Tracker.Track(httpClient);
            var estafeta = track.Estafeta("005588955041C705695585");
            var ups = track.UPS("1ZA84F19DG12759281", Isatol.Tracker.Track.Locale.es_MX);
        }
    }
}
