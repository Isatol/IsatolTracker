using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Isatol.Tracker.Models;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mime;
using System.Web;
using Tracker.Models.DHL;
using static Isatol.Tracker.Track;

namespace Isatol.Tracker
{
    public static class Helper
    {
        public static async Task<string> GetEstafetaStatus(HtmlDocument document)
        {
            return await Task.Run(() =>
            {
                HtmlNode nodeDiv = document.DocumentNode.SelectSingleNode("//div[@class='estatus']");
                if (nodeDiv != null)
                {
                    HtmlNode nodeH5 = document.DocumentNode.SelectSingleNode("//h5");
                    return nodeH5.InnerText;
                }
                return "";
            });
        }

        public static async Task<DateTime> GetEstafetaEstimateDelivery(HtmlDocument document)
        {
            return await Task.Run(() =>
            {
                HtmlNode nodeTable = document.DocumentNode.SelectSingleNode("//table[@class='table']");
                if (nodeTable != null)
                {
                    string innerText = nodeTable.InnerText.Trim();
                    string dateValue = Regex.Match(innerText, @"\d{2}[/]\d{2}[/]\d{4}").Value;
                    if (!string.IsNullOrEmpty(dateValue))
                    {
                        return Convert.ToDateTime(dateValue.Replace("/", "-"));
                    }
                    else
                    {
                        return default;
                    }
                }
                return default;
            });
        }

        public static async Task<List<Models.TrackingDetails>> GetEstafetaTrackingDetails(HtmlDocument document)
        {
            return await Task.Run(() =>
            {
                HtmlNode nodeTable = document.DocumentNode
                                        .SelectSingleNode("//table[@class='res']");
                List<Models.TrackingDetails> trackingDetails = new List<TrackingDetails>();
                if (nodeTable != null)
                {
                    List<List<string>> status = nodeTable.Descendants("tr")
                                                           .Skip(1)
                                                           .Where(tr => tr.Elements("td").Count() > 1)
                                                           .Select(tr => tr.Elements("td")
                                                           .Select(td => td.InnerHtml.Trim()).ToList())
                                                           .ToList();
                    if (status.Count > 0)
                    {
                        status.ForEach(tr =>
                        {
                            for (int i = 0; i < tr.Count; i++)
                            {
                                var eventDate = tr[0].ToString().Split('/');
                                var newEventDate = $"{eventDate[2].Substring(0, 4)}-{eventDate[1]}-{eventDate[0]} {eventDate[2].Substring(5).Replace("A. M.", "AM").Replace("P. M.", "PM")}";
                                trackingDetails.Add(new TrackingDetails
                                {
                                    Date = DateTime.Parse(newEventDate),
                                    Event = HttpUtility.HtmlDecode(tr[1].ToString()).Trim(),
                                    Messages = HttpUtility.HtmlDecode(tr[2].ToString())
                                });
                            }
                        });
                        List<TrackingDetails> distinct = trackingDetails.GroupBy(x => x.Date).Select(x => x.First()).ToList();
                        return distinct;
                    }
                }
                return trackingDetails;
            });
        }

        public static async Task<FedexResponse> GetFedexResponseAsync(string trackingNumber, HttpClient client)
        {
            string uri = "https://www.fedex.com/trackingCal/track?action=trackpackages&location=es_MX&version=1&format=json&data=";
            Models.Fedex fedex = new Fedex();
            List<TrackingInfoList> trackingInfoLists = new List<TrackingInfoList>();
            trackingInfoLists.Add(new TrackingInfoList
            {
                trackNumberInfo = new TrackNumberInfo
                {
                    trackingNumber = trackingNumber,
                    trackingCarrier = "",
                    trackingQualifier = ""
                }
            });
            TrackPackagesRequest trackPackagesRequest = new TrackPackagesRequest
            {
                processingParameters = new ProcessingParameters { },
                appType = "WTRK",
                uniqueKey = "",
                trackingInfoList = trackingInfoLists,
            };
            fedex.TrackPackagesRequest = trackPackagesRequest;
            string jsonFedexModel = Newtonsoft.Json.JsonConvert.SerializeObject(fedex);
            string newFedexURI = $"{uri}{jsonFedexModel}";
            var response = await client.PostAsync(newFedexURI, null);
            string responseObject = await response.Content.ReadAsStringAsync();
            FedexResponse fedexResponse =  Newtonsoft.Json.JsonConvert.DeserializeObject<FedexResponse>(responseObject);
            return fedexResponse;
        }

        public static async Task<UPSResponse> GetUPSResponseAsync(string trackingNumber, HttpClient client, Track.Locale locale)
        {
            try
            {
                string uri = $"https://www.ups.com/track/api/Track/GetStatus?loc={locale}";
                Models.UPS uPS = new UPS();
                uPS.TrackingNumber = new List<string>();
                uPS.TrackingNumber.Add(trackingNumber);
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(uPS);
                StringContent stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage 
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(uri),
                    Headers =
                    {
                        {"User-Agent",  "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)" }
                    },
                    Content = stringContent
                };                
                HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
                string responseObject = await response.Content.ReadAsStringAsync();
                UPSResponse uPSResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<UPSResponse>(responseObject);
                return uPSResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }

        public static async Task<DHLResponse> GetDHLResponse(string trackingNumber, HttpClient client, Locale locale)
        {
            string uri = null;
            string uri2 = null;
            switch (locale)
            {
                case Locale.es_MX:
                    uri = $"https://www.dhl.com/utapi?trackingNumber={trackingNumber}&language=es&requesterCountryCode=MX";
                    uri2 = $"https://api-eu.dhl.com/track/shipments?trackingNumber={trackingNumber}&requesterCountryCode=ES&originCountryCode=MX&language=es";
                    break;
                case Locale.en_US:
                    uri = $"https://www.dhl.com/utapi?trackingNumber={trackingNumber}&language=en&requesterCountryCode=US";
                    uri2 = $"https://api-eu.dhl.com/track/shipments?trackingNumber={trackingNumber}&requesterCountryCode=US&originCountryCode=US&language=en";
                    break;
            }    
                        
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(uri2),
                Headers =
                    {
                    // User-Agent testing purpouse
                    // {"User-Agent",  "PostmanRuntime/7.26.1" },                    
                    {"DHL-API-Key", "demo-key" },
                    {"Connection", "keep-alive" },                    
                    },
                Method = HttpMethod.Get
            };
            var response = await client.SendAsync(request);            
            string responseMessage = await response.Content.ReadAsStringAsync();
            DHLResponse dHLResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<DHLResponse>(responseMessage);
            return dHLResponse;
        }
    }
}
