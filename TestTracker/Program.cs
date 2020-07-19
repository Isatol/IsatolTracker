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
            var estafeta = track.Estafeta("");
            var ups = track.UPS("", Isatol.Tracker.Track.Locale.es_MX);
        }
    }
}
