using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrackerXamarinDemo.Services
{
    public class TrackService
    {
        Isatol.Tracker.Track Track;
        public TrackService()
        {
            HttpClient client = new HttpClient();
            Track = new Isatol.Tracker.Track(client);
        }

        public async Task<Isatol.Tracker.Models.TrackingModel> GetTracking(int companyID, string trackingNumber)
        {
            switch (companyID)
            {
                case 1:
                    return await Track.EstafetaAsync(trackingNumber);                    
                case 2:
                    return await Track.FedexAsync(trackingNumber);
                case 3:
                    return await Track.UPSAsync(trackingNumber, Isatol.Tracker.Track.Locale.es_MX);
                case 4:
                    return await Track.DHLAsync(trackingNumber, Isatol.Tracker.Track.Locale.es_MX);
                default:
                    return new Isatol.Tracker.Models.TrackingModel();                    
            }
        }
    }
}
