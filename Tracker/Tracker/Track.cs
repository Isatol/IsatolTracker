using HtmlAgilityPack;
using ShippingTrackingUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IsatolTracker.Models;

namespace IsatolTracker
{
    /// <summary>
    /// Contains methods to track packages
    /// </summary>
    public class Track
    {
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Track Fedex packages
        /// </summary>
        /// <param name="trackingNumber">The tracking ID</param>
        /// <returns>Details of the package history</returns>
        public static IsatolTracker.Models.TrackingModel Fedex(string trackingNumber)
        {            
            FedExTracking fedExTracking = new FedExTracking(trackingNumber);            
            ShippingResult shippingResult = fedExTracking.GetTrackingResult();
            Models.TrackingModel tracking = new Models.TrackingModel();
            tracking.TrackingDetails = new List<TrackingDetails>();
            tracking.Status = shippingResult.Status;
            if (shippingResult.StatusCode == "ERROR")
            {
                return default;
            }
            else if (shippingResult.StatusCode == "D")
            {
                tracking.Delivered = true;
                return tracking;
            }
            else
            {
                if (!string.IsNullOrEmpty(shippingResult.ScheduledDeliveryDate))
                {
                    tracking.EstimateDelivery = Convert.ToDateTime(shippingResult.ScheduledDeliveryDate);
                }
                shippingResult.TrackingDetails.ForEach(track => 
                {
                    tracking.TrackingDetails.Add(new Models.TrackingDetails 
                    {
                        Date = Convert.ToDateTime(track.EventDateTime),
                        Event = track.Event,
                        Messages = track.EventAddress
                    });
                });
                return tracking;
            }
        }

        /// <summary>
        /// Track Fedex packages asynchronously
        /// </summary>
        /// <param name="trackingNumber">The tracking ID</param>
        /// <returns>Details of the package history</returns>
        public static async  Task<IsatolTracker.Models.TrackingModel> FedexAsync(string trackingNumber)
        {
            return await Task.Run(() =>
            {
                FedExTracking fedExTracking = new FedExTracking(trackingNumber);
                ShippingResult shippingResult = fedExTracking.GetTrackingResult();
                Models.TrackingModel tracking = new Models.TrackingModel();
                tracking.TrackingDetails = new List<TrackingDetails>();
                tracking.Status = shippingResult.Status;
                if (shippingResult.StatusCode == "ERROR")
                {
                    return default;
                }
                else if (shippingResult.StatusCode == "D")
                {
                    tracking.Delivered = true;
                    return tracking;
                }
                else
                {
                    if (!string.IsNullOrEmpty(shippingResult.ScheduledDeliveryDate))
                    {
                        tracking.EstimateDelivery = Convert.ToDateTime(shippingResult.ScheduledDeliveryDate);
                    }
                    shippingResult.TrackingDetails.ForEach(track =>
                    {
                        tracking.TrackingDetails.Add(new Models.TrackingDetails
                        {
                            Date = Convert.ToDateTime(track.EventDateTime),
                            Event = track.Event,
                            Messages = track.EventAddress
                        });
                    });
                    return tracking;
                }
            });
        }

        /// <summary>
        /// Track UPS packages
        /// </summary>
        /// <param name="trackingNumber">The tracking number</param>
        /// <returns>Details of the package history</returns>
        public static IsatolTracker.Models.TrackingModel UPS(string trackingNumber)
        {
            UPSTracking uPSTracking = new UPSTracking(trackingNumber);
            Models.TrackingModel tracking = new Models.TrackingModel();
            tracking.TrackingDetails = new List<TrackingDetails>();

            ShippingResult shippingResult = uPSTracking.GetTrackingResult();
            tracking.Status = shippingResult.Status;
            if (shippingResult.StatusCode == "Error")
            {
                return default;
            }
            else if(shippingResult.StatusCode == "D")
            {
                tracking.Delivered = true;                
                return tracking;
            }
            else
            {
                if (!string.IsNullOrEmpty(shippingResult.ScheduledDeliveryDate))
                {
                    tracking.EstimateDelivery = Convert.ToDateTime(shippingResult.ScheduledDeliveryDate);
                }                
                shippingResult.TrackingDetails.ForEach(track =>
                {
                    tracking.TrackingDetails.Add(new Models.TrackingDetails
                    {
                        Date = Convert.ToDateTime(track.EventDateTime),
                        Event = track.Event,
                        Messages = track.EventAddress
                    });
                });
                return tracking;
            }
        }

        /// <summary>
        /// Track UPS packages asynchronously
        /// </summary>
        /// <param name="trackingNumber"></param>
        /// <returns>Details of the package history</returns>
        public static async Task<IsatolTracker.Models.TrackingModel> UPSAsync(string trackingNumber)
        {
            return await Task.Run(() =>
            {
                UPSTracking uPSTracking = new UPSTracking(trackingNumber);
                Models.TrackingModel tracking = new Models.TrackingModel();
                tracking.TrackingDetails = new List<TrackingDetails>();

                ShippingResult shippingResult = uPSTracking.GetTrackingResult();
                tracking.Status = shippingResult.Status;
                if (shippingResult.StatusCode == "Error")
                {
                    return default;
                }
                else if (shippingResult.StatusCode == "D")
                {
                    tracking.Delivered = true;                    
                    return tracking;
                }
                else
                {
                    if (!string.IsNullOrEmpty(shippingResult.ScheduledDeliveryDate))
                    {
                        tracking.EstimateDelivery = Convert.ToDateTime(shippingResult.ScheduledDeliveryDate);
                    }
                    shippingResult.TrackingDetails.ForEach(track =>
                    {
                        tracking.TrackingDetails.Add(new Models.TrackingDetails
                        {
                            Date = Convert.ToDateTime(track.EventDateTime),
                            Event = track.Event,
                            Messages = track.EventAddress
                        });
                    });
                    return tracking;
                }
            });
        }

        /// <summary>
        /// Track Estafeta packages
        /// </summary>
        /// <param name="trackingNumber">The tracking number or guide number</param>
        /// <returns>Details of the package history</returns>
        public static Models.TrackingModel Estafeta(string trackingNumber)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "wayBill", trackingNumber },
                { "waybillType", trackingNumber.Length == 10 ? "0" : "1" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(keyValuePairs);
            HttpResponseMessage response = client.PostAsync("https://rastreositecorecms.azurewebsites.net/Tracking/searchWayBill/", content).Result;
            string responseString = response.Content.ReadAsStringAsync().Result;
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(responseString);
            TrackingModel trackingModel = new TrackingModel();
            trackingModel.TrackingDetails = new List<TrackingDetails>();
            string status = Helper.GetEstafetaStatus(document).Result;
            trackingModel.Status = string.IsNullOrEmpty(status) ? "ERROR" : status;
            if(status == "ENTREGADO")
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
        public static async Task<Models.TrackingModel> EstafetaAsync(string trackingNumber) 
        {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "wayBill", trackingNumber },
                { "waybillType", trackingNumber.Length == 10 ? "0" : "1" }
            };
                FormUrlEncodedContent content = new FormUrlEncodedContent(keyValuePairs);
                HttpResponseMessage response = await client.PostAsync("https://rastreositecorecms.azurewebsites.net/Tracking/searchWayBill/", content);
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
