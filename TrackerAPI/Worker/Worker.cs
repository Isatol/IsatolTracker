using Isatol.Tracker;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using TrackerAPI.Models;

namespace TrackerAPI.Worker
{
    public class Worker : BackgroundService
    {
        private readonly TrackerDAL.Tracking _tracking;        
        private readonly PushServiceClient _pushServiceClient;
        private readonly Isatol.Tracker.Track _track;
        private readonly TrackerDAL.TrackerSystem _system;
        private readonly Models.EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Worker(Helper.ConnectionStrings connectionStrings, IOptions<PushNotification> pushNotification, Isatol.Tracker.Track track, IOptions<EmailSettings> emailSettings, PushServiceClient pushServiceClient, IWebHostEnvironment webHostEnvironment)
        {
            _tracking = new TrackerDAL.Tracking(connectionStrings.GetConnectionString());
            _pushServiceClient = pushServiceClient;
            _pushServiceClient.DefaultAuthentication = new Lib.Net.Http.WebPush.Authentication.VapidAuthentication(pushNotification.Value.PublicKey, pushNotification.Value.PrivateKey) 
            {
                Subject = $"mailto:{emailSettings.Value.Email}"
            };
            _track = track;
            _system = new TrackerDAL.TrackerSystem(connectionStrings.GetConnectionString());
            _emailSettings = emailSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Execute();
                await Task.Delay(10800000, cancellationToken);
            }
        }

        private async Task Execute()
        {
            List<TrackerDAL.Models.Package> packages = await _tracking.GetPackages();            
            if(packages.Count > 0)
            {
                string rootPath = _webHostEnvironment.ContentRootPath;
                string templatePath = Path.Combine(rootPath, "Templates", "SendNotification.html");
                string template = await System.IO.File.ReadAllTextAsync(templatePath);
                packages.ForEach(async p => 
                {
                    TrackerDAL.Models.Users user = await _system.GetUser(p.UsersID);
                    TrackerDAL.Models.Company company = await _tracking.GetCompany(p.CompanyID);
                    TrackerDAL.Models.LastPackageUpdate lastPackageUpdate = await _tracking.GetLastPackageUpdate(p.PackageID);                    
                    switch (company.CompanyID)
                    {
                        case 1:
                            Isatol.Tracker.Models.TrackingModel estafeta = await _track.EstafetaAsync(p.TrackingNumber);
#if DEBUG
                            estafeta.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
                            {
                                Date = DateTime.Now,
                                Event = "En proceso en ir a la casa",
                                Messages = ""
                            });
#endif
                            if (lastPackageUpdate == null)
                            {                                
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate 
                                {
                                    Date = estafeta.TrackingDetails[0].Date,
                                    Event = estafeta.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if(lastPackageUpdate.Date != estafeta.TrackingDetails[0].Date)
                            {
                                await SendPushNotification(p.UsersID, company.Name, company.Logo, p.Name, $"{estafeta.TrackingDetails[0].Event} {estafeta.TrackingDetails[0].Messages}");
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate 
                                {
                                    Date = estafeta.TrackingDetails[0].Date,
                                    Event = estafeta.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                                if (user.ReceiveEmails)
                                {
                                    await SendEmailNotification(user.Email, new Dictionary<string, string>
                                       {
                                           {"#nombre#", user.Name },
                                           {"#paquete#", p.Name },
                                           {"#evento#", $"{estafeta.TrackingDetails[0].Event} {estafeta.TrackingDetails[0].Messages}" }
                                       }, template, true);
                                }
                            }
                            break;
                        case 2:
                            Isatol.Tracker.Models.TrackingModel fedex = await _track.FedexAsync(p.TrackingNumber);
                            if(lastPackageUpdate == null)
                            {
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate 
                                {
                                    Date = fedex.TrackingDetails[0].Date,
                                    Event = fedex.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if(lastPackageUpdate.Date != fedex.TrackingDetails[0].Date)
                            {
                                await SendPushNotification(p.UsersID, company.Name, company.Logo, p.Name, $"{fedex.TrackingDetails[0].Event} {fedex.TrackingDetails[0].Messages}");
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate 
                                {
                                    Date = fedex.TrackingDetails[0].Date,
                                    Event = fedex.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                                if (user.ReceiveEmails)
                                {
                                    await SendEmailNotification(user.Email, new Dictionary<string, string>
                                       {
                                           {"#nombre#", user.Name },
                                           {"#paquete#", p.Name },
                                           {"#evento#", $"{fedex.TrackingDetails[0].Event} {fedex.TrackingDetails[0].Messages}" }
                                       }, template, true);
                                }
                            }
                            break;
                        case 3:
                            Isatol.Tracker.Models.TrackingModel ups = await _track.UPSAsync(p.TrackingNumber, Track.Locale.es_MX);
                            if(lastPackageUpdate == null)
                            {
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate 
                                {
                                    Date = ups.TrackingDetails[0].Date,
                                    Event = ups.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if(lastPackageUpdate.Date != ups.TrackingDetails[0].Date)
                            {
                                await SendPushNotification(p.UsersID, company.Name, company.Logo, p.Name, $"{ups.TrackingDetails[0].Event} {ups.TrackingDetails[0].Messages}");
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = ups.TrackingDetails[0].Date,
                                    Event = ups.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                                if (user.ReceiveEmails)
                                {
                                    await SendEmailNotification(user.Email, new Dictionary<string, string>
                                       {
                                           {"#nombre#", user.Name },
                                           {"#paquete#", p.Name },
                                           {"#evento#", $"{ups.TrackingDetails[0].Event} {ups.TrackingDetails[0].Messages}" }
                                       }, template, true);
                                }
                            }
                            break;
                        case 4:
                            Isatol.Tracker.Models.TrackingModel dhl = await _track.DHLAsync(p.TrackingNumber, Track.Locale.es_MX);
//#if DEBUG
//                            dhl.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
//                            {
//                                Date = DateTime.Now,
//                                Event = "En proceso en ir a la casa",
//                                Messages = ""
//                            });
//#endif

                            if (lastPackageUpdate == null)
                            {
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate 
                                {
                                    Date = dhl.TrackingDetails[0].Date,
                                    Event = dhl.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if(lastPackageUpdate.Date != dhl.TrackingDetails[0].Date)
                            {
                                await SendPushNotification(p.UsersID, company.Name, company.Logo, p.Name, $"{dhl.TrackingDetails[0].Event} {dhl.TrackingDetails[0].Messages}");
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate 
                                {
                                    Date = dhl.TrackingDetails[0].Date,
                                    Event = dhl.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                                if (user.ReceiveEmails)
                                {
                                    await SendEmailNotification(user.Email, new Dictionary<string, string>
                                       {
                                           {"#nombre#", user.Name },
                                           {"#paquete#", p.Name },
                                           {"#evento#", $"{dhl.TrackingDetails[0].Event} {dhl.TrackingDetails[0].Messages}" }
                                       }, template, true);
                                }
                            }
                            break;
                    }                    
                });
            }
        }

        private async Task SendPushNotification(int userID, string company, string companyLogo, string packageName, string packageEvent)
        {
            List<TrackerDAL.Models.Notification> notifications = await _system.GetUserNotifications(userID);
            if(notifications.Count > 0)
            {
                PushMessage pushMessage = new Models.Notification 
                {
                    Icon = companyLogo,
                    Title = string.IsNullOrEmpty(packageName) ? company : packageName,
                    Body = packageEvent,
                    Data = new Dictionary<string, object> 
                    {
                        {"userID", userID }
                    }                    
                }.ToPushMessage();

                notifications.ForEach(async notification =>
                {
                    PushSubscription pushSubscription = new PushSubscription 
                    {
                        Endpoint = notification.Endpoint,
                        Keys = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(notification.Keys)
                    };
                    await _pushServiceClient.RequestPushMessageDeliveryAsync(pushSubscription, pushMessage);
                });
            }
        }
        
        private async Task SendEmailNotification(string recipient, Dictionary<string, string> keyValuePairsReplaceItems, string body, bool isBodyHTML)
        {
            try
            {
                string aux = "";
                using (MailMessage mm = new MailMessage())
                {
                    aux = body;
                    foreach (var item in keyValuePairsReplaceItems)
                    {
                        aux = aux.Replace(item.Key, item.Value);
                    }
                    mm.Subject = _emailSettings.Subject;
                    mm.From = new MailAddress(_emailSettings.Email);
                    mm.To.Add(recipient);
                    mm.Body = aux;
                    mm.IsBodyHtml = isBodyHTML;
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = _emailSettings.SMTPServer;
                        smtpClient.Port = _emailSettings.Port;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
                        smtpClient.EnableSsl = _emailSettings.UseSSL;
                        if (_emailSettings.UseSSL)
                        {
                            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        }
                        await smtpClient.SendMailAsync(mm);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
