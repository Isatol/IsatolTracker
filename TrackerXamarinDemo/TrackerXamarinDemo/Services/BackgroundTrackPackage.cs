using Isatol.Tracker;
using Matcha.BackgroundService;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TrackerXamarinDemo.Services
{
    public class BackgroundTrackPackage : IPeriodicTask
    {
        public TimeSpan Interval { get; set; }
        private Database.Database Database;
        HttpClient HttpClient;
        public BackgroundTrackPackage(int seconds, Database.Database database)
        {
            Interval = TimeSpan.FromSeconds(seconds);
            Database = database;
            HttpClient = new HttpClient();
        }

        public Task<bool> StartJob()
        {
            return Task.Run(async () => 
            {
                var packages = await Database.GetPackages();
                TrackService trackService = new TrackService();
                var minutes = await Database.GetMinutes();
                if(minutes == null)
                {
                    await Database.InsertMinutes(new TrackerXamarinDemo.Database.Database.Minutes
                    {
                        DateTime = DateTime.Now.AddMinutes(5)
                    });
                }
                else
                {
                    if(DateTime.Now >= minutes.DateTime)
                    {
                        await CheckPackages(packages, trackService);
                        await Database.UpdateMinutes(new TrackerXamarinDemo.Database.Database.Minutes
                        {
                            DateTime = DateTime.Now.AddMinutes(5),
                            MinutesID = minutes.MinutesID
                        });
                    }
                }
                return true;
            });
        }

        private async Task CheckPackages(List<Database.Database.Package> packages, TrackService trackService)
        {
            if (packages.Count > 0)
            {
                packages.ForEach(async package =>
                {
                    var lastPackageUpdate = await Database.GetLastPackageUpdate(package.PackageID);
                    var trackingModel = await trackService.GetTracking(package.CompanyID, package.TrackingNumber);
#if DEBUG
                    trackingModel.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
                    {
                        Date = DateTime.Now,
                        Event = "Por solicitud, el paquete está moviéndose en depósito a agente no UPS para liberación. / Su paquete fue liberado por la agencia aduanal.	",
                        Messages = ""
                    });
#endif
                    if (lastPackageUpdate.Date == null && trackingModel.TrackingDetails.Count > 0)
                    {
                        await Database.UpdateLastPackageUpdate(new TrackerXamarinDemo.Database.Database.LastPackageUpdate
                        {
                            Date = trackingModel.TrackingDetails[0].Date,
                            LastPackageUpdateID = lastPackageUpdate.LastPackageUpdateID,
                            Event = trackingModel.TrackingDetails[0].Event,
                            Messages = trackingModel.TrackingDetails[0].Messages,
                            PackageID = package.PackageID
                        });
                    }
                    if (trackingModel.TrackingDetails.Count > 0)
                    {
                        if (lastPackageUpdate.Date != null && lastPackageUpdate.Date != trackingModel.TrackingDetails[0].Date)
                        {
                            await Database.UpdateLastPackageUpdate(new TrackerXamarinDemo.Database.Database.LastPackageUpdate
                            {
                                Date = trackingModel.TrackingDetails[0].Date,
                                LastPackageUpdateID = lastPackageUpdate.LastPackageUpdateID,
                                Event = trackingModel.TrackingDetails[0].Event,
                                Messages = trackingModel.TrackingDetails[0].Messages,
                                PackageID = package.PackageID
                            });
                            CrossLocalNotifications.Current.Show(package.Name, trackingModel.TrackingDetails[0].Event + trackingModel.TrackingDetails[0].Messages);
                        }
                    }
                });
            }
        }
    }
}
