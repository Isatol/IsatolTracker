using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Isatol.Tracker.Models;
using System.Linq.Expressions;
using System.Net.Http.Formatting;
using System.Web;
using System.Globalization;

namespace Isatol.Tracker
{
    /// <summary>
    /// Contains methods to track packages
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Response language
        /// </summary>
        public enum Locale
        {
            /// <summary>
            /// Español México
            /// </summary>
            es_MX,
            /// <summary>
            /// English US
            /// </summary>
            en_US
        }
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initialize a new instance of the Tracker class to track other packages
        /// </summary>
        /// <param name="httpClient"></param>
        public Track(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Track Fedex packages
        /// </summary>
        /// <param name="trackingNumber">The tracking ID</param>
        /// <returns>Details of the package history</returns>
        public TrackingModel Fedex(string trackingNumber)
        {
            FedexResponse fedexResponse = Helper.GetFedexResponseAsync(trackingNumber, _httpClient).Result;
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.TrackingDetails = new List<TrackingDetails>();
            if (fedexResponse.TrackPackagesResponse.successful) 
            {
                PackageList fedexPackage = fedexResponse.TrackPackagesResponse.PackageList[0];
                trackingModel.Status = fedexPackage.KeyStatus;
                trackingModel.Delivered = fedexPackage.KeyStatusCD == "DL";
                if (DateTime.TryParse(fedexPackage.displayEstDeliveryDateTime, out DateTime displayEstDeliveryDateTime))
                {
                    trackingModel.EstimateDelivery = displayEstDeliveryDateTime;
                }
                fedexPackage.ScanEventList.ForEach(package =>
                {
                    DateTime? scanDate = null;
                    if (package.Date != null && package.Time != null)
                    {
                        scanDate = package.Date + package.Time;
                    }
                    trackingModel.TrackingDetails.Add(new TrackingDetails
                    {
                        Date = scanDate,
                        Event = package.Status,
                        Messages = package.ScanLocation
                    });
                });                
            }
            return trackingModel;
        }

        /// <summary>
        /// Track Fedex packages asynchronously
        /// </summary>
        /// <param name="trackingNumber">The tracking ID</param>
        /// <returns>Details of the package history</returns>
        public async Task<TrackingModel> FedexAsync(string trackingNumber) 
        {
            FedexResponse fedexResponse = await Helper.GetFedexResponseAsync(trackingNumber, _httpClient);
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.TrackingDetails = new List<TrackingDetails>();
            if (fedexResponse.TrackPackagesResponse.successful)
            {
                PackageList fedexPackage = fedexResponse.TrackPackagesResponse.PackageList[0];
                trackingModel.Status = fedexPackage.KeyStatus;
                trackingModel.Delivered = fedexPackage.KeyStatusCD == "DL";
                if (DateTime.TryParse(fedexPackage.displayEstDeliveryDateTime, out DateTime displayEstDeliveryDateTime))
                {
                    trackingModel.EstimateDelivery = displayEstDeliveryDateTime;
                }
                fedexPackage.ScanEventList.ForEach(package =>
                {
                    DateTime? scanDate = null;
                    if (package.Date != null && package.Time != null)
                    {
                        scanDate = package.Date + package.Time;
                    }
                    trackingModel.TrackingDetails.Add(new TrackingDetails
                    {
                        Date = scanDate,
                        Event = package.Status,
                        Messages = package.ScanLocation
                    });
                });
            }
            return trackingModel;
        }

        /// <summary>
        /// Track UPS Package
        /// </summary>
        /// <param name="trackingNumber">The tracking number</param>
        /// <param name="locale">Specify language of details</param>
        /// <returns></returns>
        public TrackingModel UPS(string trackingNumber, Locale locale)
        {
            UPSResponse uPSResponse = Helper.GetUPSResponseAsync(trackingNumber, _httpClient, locale).Result;
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.TrackingDetails = new List<TrackingDetails>();
            if (uPSResponse.StatusCode == "200")
            {
                var trackDetails = uPSResponse.TrackDetails[0];
                trackingModel.Delivered = trackDetails.PackageStatusType == "D";
                trackingModel.Status = trackDetails.PackageStatus;
                if (DateTime.TryParse(trackDetails.EstimatedArrival, out DateTime estimatedArrival))
                {
                    trackingModel.EstimateDelivery = estimatedArrival;
                }
                trackDetails.ShipmentProgressActivities.ForEach(activity =>
                {
                    DateTime? scanDate = null;
                    if (activity.Date != null && activity.Time != null)
                    {
                        scanDate = DateTime.ParseExact(activity.Date, "MM/dd/yyyy", CultureInfo.InvariantCulture) + DateTime.Parse(activity.Time.Replace(".", "")).TimeOfDay;
                    }
                    trackingModel.TrackingDetails.Add(new TrackingDetails
                    {
                        Date = scanDate,
                        Event = HttpUtility.HtmlDecode(activity.ActivityScan).Trim(),
                        Messages = activity.Location
                    });
                });
            }
            return trackingModel;
        }

        /// <summary>
        /// Track UPS Package asynchronously
        /// </summary>
        /// <param name="trackingNumber">The tracking number</param>
        /// <param name="locale">Specify language of details</param>
        /// <returns></returns>
        public async Task<TrackingModel> UPSAsync(string trackingNumber, Locale locale) 
        {
            UPSResponse uPSResponse = await Helper.GetUPSResponseAsync(trackingNumber, _httpClient, locale);
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.TrackingDetails = new List<TrackingDetails>();
            if (uPSResponse.StatusCode == "200")
            {
                var trackDetails = uPSResponse.TrackDetails[0];
                trackingModel.Delivered = trackDetails.PackageStatusType == "D";
                trackingModel.Status = trackDetails.PackageStatus;
                if (DateTime.TryParse(trackDetails.EstimatedArrival, out DateTime estimatedArrival))
                {
                    trackingModel.EstimateDelivery = estimatedArrival;
                }
                trackDetails.ShipmentProgressActivities.ForEach(activity =>
                {
                    DateTime? scanDate = null; 
                    if (activity.Date != null && activity.Time != null)
                    {
                        if (locale == Locale.en_US)
                        {
                            string[] splitDate = activity.Date.Split('/');
                            string newDate = $"{splitDate[2]}-{splitDate[0]}-{splitDate[1]} {activity.Time.Replace(".", "")}";
                            scanDate = Convert.ToDateTime(newDate);

                        }
                        else
                        {
                            string[] splitDate = activity.Date.Split('/');
                            string newDate = $"{splitDate[2]}-{splitDate[1]}-{splitDate[0]} {activity.Time.Replace(".", "")}";
                            scanDate = Convert.ToDateTime(newDate);
                        }
                    }
                    trackingModel.TrackingDetails.Add(new TrackingDetails
                    {
                        Date = scanDate,
                        Event = HttpUtility.HtmlDecode(activity.ActivityScan).Trim(),
                        Messages = activity.Location
                    });
                });
            }
            return trackingModel;
        }

        /// <summary>
        /// Track Estafeta packages
        /// </summary>
        /// <param name="trackingNumber">The tracking number or guide number</param>
        /// <returns>Details of the package history</returns>
        public Models.TrackingModel Estafeta(string trackingNumber)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "wayBill", trackingNumber },
                { "waybillType", trackingNumber.Length == 10 ? "0" : "1" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage response =  _httpClient.PostAsync("https://rastreositecorecms.azurewebsites.net/Tracking/searchWayBill/", content).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(responseString);
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.TrackingDetails = new List<TrackingDetails>();
            string status = Helper.GetEstafetaStatus(document).Result;
            trackingModel.Status = string.IsNullOrEmpty(status) ? "ERROR" : status;
            if (status == "ENTREGADO")
            {
                trackingModel.Delivered = true;
                List<TrackingDetails> trackingDetails = Helper.GetEstafetaTrackingDetails(document).Result;
                trackingModel.TrackingDetails.AddRange(trackingDetails);
            }
            else
            {
                trackingModel.EstimateDelivery = Helper.GetEstafetaEstimateDelivery(document).Result;
                List<TrackingDetails> trackingDetails = Helper.GetEstafetaTrackingDetails(document).Result;
                trackingModel.TrackingDetails.AddRange(trackingDetails);
            }
            return trackingModel;
        }

        /// <summary>
        /// Track Estafeta packages
        /// </summary>
        /// <param name="trackingNumber">The tracking number or guide number</param>
        /// <returns>Details of the package history</returns>
        public async Task<Models.TrackingModel> EstafetaAsync(string trackingNumber)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "wayBill", trackingNumber },
                { "waybillType", trackingNumber.Length == 10 ? "0" : "1" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage response = await _httpClient.PostAsync("https://rastreositecorecms.azurewebsites.net/Tracking/searchWayBill/", content);
            string responseString = await response.Content.ReadAsStringAsync();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(responseString);
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.TrackingDetails = new List<TrackingDetails>();
            string status = await Helper.GetEstafetaStatus(document);
            trackingModel.Status = string.IsNullOrEmpty(status) ? "ERROR" : status;
            if (status == "ENTREGADO")
            {
                trackingModel.Delivered = true;
                List<TrackingDetails> trackingDetails = await Helper.GetEstafetaTrackingDetails(document);
                trackingModel.TrackingDetails.AddRange(trackingDetails);
            }
            else
            {
                trackingModel.EstimateDelivery = await Helper.GetEstafetaEstimateDelivery(document);
                List<TrackingDetails> trackingDetails = await Helper.GetEstafetaTrackingDetails(document);
                trackingModel.TrackingDetails.AddRange(trackingDetails);
            }
            return trackingModel;
        }
    }
}
