using System;

namespace TestTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var fedex = IsatolTracker.Track.Fedex("");
            var ups = IsatolTracker.Track.UPS("");
            var estafeta = IsatolTracker.Track.Estafeta("");
        }
    }
}
