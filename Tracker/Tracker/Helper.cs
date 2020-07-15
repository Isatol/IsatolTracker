using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IsatolTracker.Models;

namespace IsatolTracker
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
                                trackingDetails.Add(new TrackingDetails
                                {
                                    Date = DateTime.Parse(tr[0].ToString()),
                                    Event = tr[1].ToString(),
                                    Messages = tr[2].ToString()
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
    }
}
