using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.LocalNotifications;
using TrackerXamarinDemo.Services;

namespace TrackerXamarinDemo.Droid
{
    [Service(Name = "com.isatol.packagetracker.CheckPackagesJob", Permission = "android.permission.BIND_JOB_SERVICE")]
    public class CheckPackagesJob : JobService
    {
        public override bool OnStartJob(JobParameters @params)
        {
            Task.Run(async () =>
            {
                var packages = await App.Database.GetPackages();
                TrackService trackService = new TrackService();
                var minutes = await App.Database.GetMinutes();
                if (minutes == null)
                {
                    await App.Database.InsertMinutes(new TrackerXamarinDemo.Database.Database.Minutes
                    {
                        DateTime = DateTime.Now.AddMinutes(20)
                    });
                }
                else
                {
                    if (DateTime.Now >= minutes.DateTime)
                    {
                        await CheckPackages(packages, trackService);
                        await App.Database.UpdateMinutes(new TrackerXamarinDemo.Database.Database.Minutes
                        {
                            DateTime = DateTime.Now.AddMinutes(20),
                            MinutesID = minutes.MinutesID
                        });
                    }
                }                
            });
            JobFinished(@params, true);
            return true;
        }

        private async Task CheckPackages(List<TrackerXamarinDemo.Database.Database.Package> packages, TrackService trackService)
        {
            if (packages.Count > 0)
            {
                packages.ForEach(async package =>
                {
                    var lastPackageUpdate = await App.Database.GetLastPackageUpdate(package.PackageID);
                    var trackingModel = await trackService.GetTracking(package.CompanyID, package.TrackingNumber);
//#if DEBUG
//                    trackingModel.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
//                    {
//                        Date = DateTime.Now,
//                        Event = "Por solicitud, el paquete está moviéndose en depósito a agente no UPS para liberación. / Su paquete fue liberado por la agencia aduanal.	",
//                        Messages = ""
//                    });
//#endif
                    if(lastPackageUpdate == null && trackingModel.TrackingDetails.Count > 0)
                    {
                        await App.Database.InsertLastPackageUpdate(new TrackerXamarinDemo.Database.Database.LastPackageUpdate
                        {
                            PackageID = package.PackageID,
                            Date = trackingModel.TrackingDetails[0].Date,
                            Event = trackingModel.TrackingDetails[0].Event,
                            Messages = trackingModel.TrackingDetails[0].Messages
                        });
                    }
                    if (lastPackageUpdate.Date == null && trackingModel.TrackingDetails.Count > 0)
                    {
                        await App.Database.UpdateLastPackageUpdate(new TrackerXamarinDemo.Database.Database.LastPackageUpdate
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
                            await App.Database.UpdateLastPackageUpdate(new TrackerXamarinDemo.Database.Database.LastPackageUpdate
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

        public override bool OnStopJob(JobParameters @params)
        {
            return true;
        }
    }
}