using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Isatol.Tracker;
using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PackageWorker.Models;

namespace PackageWorker
{
    public class Worker : BackgroundService
    {        
        private readonly ILogger<Worker> _logger;
        private readonly TrackerDAL.TrackerSystem _system;
        private readonly TrackerDAL.Tracking _tracking;
        private readonly IHostEnvironment _hostingEnvironment;
        private Isatol.Tracker.Track _track;
        private PushServiceClient _pushServiceClient;
        private readonly EmailSettings _emailSettings;
        public Worker(ILogger<Worker> logger, TrackerDAL.TrackerSystem system, TrackerDAL.Tracking tracking, IOptions<PushNotification> pushNotification, IOptions<EmailSettings> emailSettings, IHostEnvironment hostEnvironment)
        {
         HttpClient _client = new HttpClient();
            _tracking = tracking;
            _system = system;
            _hostingEnvironment = hostEnvironment;
            _pushServiceClient = new PushServiceClient(_client);
            _pushServiceClient.DefaultAuthentication = new Lib.Net.Http.WebPush.Authentication.VapidAuthentication(pushNotification.Value.PublicKey, pushNotification.Value.PrivateKey) 
            {
                Subject = $"mailto:{emailSettings.Value.Email}"
            };
            _track = new Isatol.Tracker.Track(_client);
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Execute();
                await Task.Delay(1800000, stoppingToken);
            }
        }

        private async Task Execute()
        {
            await _system.AddLog("DESDE IIS WORKER");
            List<TrackerDAL.Models.Package> packages = await _tracking.GetPackages();
            if(packages.Count > 0)
            {
                string rootPath = _hostingEnvironment.ContentRootPath;
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
                            if (p.UsersID == 1)
                            {
                                estafeta.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
                                {
                                    Date = DateTime.Now,
                                    Event = "Por solicitud, el paquete está moviéndose en depósito a agente no UPS para liberación. / Su paquete fue liberado por la agencia aduanal.	",
                                    Messages = ""
                                });
                            }
#endif
                            if (lastPackageUpdate == null && estafeta.TrackingDetails.Count > 0)
                            {
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = estafeta.TrackingDetails[0].Date,
                                    Event = estafeta.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if (lastPackageUpdate.Date == null && estafeta.TrackingDetails.Count > 0)
                            {
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = estafeta.TrackingDetails[0].Date,
                                    Event = estafeta.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            if (estafeta.TrackingDetails.Count > 0)
                            {
                                if (lastPackageUpdate.Date != null && lastPackageUpdate.Date != estafeta.TrackingDetails[0].Date)
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
                            }

                            break;
                        case 2:
                            Isatol.Tracker.Models.TrackingModel fedex = await _track.FedexAsync(p.TrackingNumber);
                            //#if DEBUG
                            //                            if (p.UsersID == 1)
                            //                            {
                            //                                fedex.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
                            //                                {
                            //                                    Date = DateTime.Now,
                            //                                    Event = "Por solicitud, el paquete está moviéndose en depósito a agente no UPS para liberación. / Su paquete fue liberado por la agencia aduanal.	",
                            //                                    Messages = ""
                            //                                });
                            //                            }
                            //#endif
                            if (lastPackageUpdate == null && fedex.TrackingDetails.Count > 0)
                            {
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = fedex.TrackingDetails[0].Date,
                                    Event = fedex.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if (lastPackageUpdate.Date == null && fedex.TrackingDetails.Count > 0)
                            {
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = fedex.TrackingDetails[0].Date,
                                    Event = fedex.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            if (fedex.TrackingDetails.Count > 0)
                            {
                                if (lastPackageUpdate.Date != null && lastPackageUpdate.Date != fedex.TrackingDetails[0].Date)
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
                            }

                            break;
                        case 3:
                            Isatol.Tracker.Models.TrackingModel ups = await _track.UPSAsync(p.TrackingNumber, Track.Locale.es_MX);
                            //#if DEBUG
                            //                            if (p.UsersID == 1)
                            //                            {
                            //                                ups.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
                            //                                {
                            //                                    Date = DateTime.Now,
                            //                                    Event = "Por solicitud, el paquete está moviéndose en depósito a agente no UPS para liberación. / Su paquete fue liberado por la agencia aduanal.	",
                            //                                    Messages = ""
                            //                                });
                            //                            }
                            //#endif
                            if (lastPackageUpdate == null && ups.TrackingDetails.Count > 0)
                            {
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = ups.TrackingDetails[0].Date,
                                    Event = ups.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if (lastPackageUpdate.Date == null && ups.TrackingDetails.Count > 0)
                            {
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = ups.TrackingDetails[0].Date,
                                    Event = ups.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            if (ups.TrackingDetails.Count > 0)
                            {
                                if (lastPackageUpdate.Date != null && lastPackageUpdate.Date != ups.TrackingDetails[0].Date)
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
                            }

                            break;
                        case 4:
                            Isatol.Tracker.Models.TrackingModel dhl = await _track.DHLAsync(p.TrackingNumber, Track.Locale.es_MX);
                            //#if DEBUG
                            //                            if (p.UsersID == 1)
                            //                            {
                            //                                dhl.TrackingDetails.Insert(0, new Isatol.Tracker.Models.TrackingDetails
                            //                                {
                            //                                    Date = DateTime.Now,
                            //                                    Event = "Por solicitud, el paquete está moviéndose en depósito a agente no UPS para liberación. / Su paquete fue liberado por la agencia aduanal.	",
                            //                                    Messages = ""
                            //                                });
                            //                            }
                            //#endif
                            if (lastPackageUpdate == null && dhl.TrackingDetails.Count > 0)
                            {
                                await _tracking.InsertLastPackageUpdate(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = dhl.TrackingDetails[0].Date,
                                    Event = dhl.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            else if (lastPackageUpdate.Date == null && dhl.TrackingDetails.Count > 0)
                            {
                                await _tracking.UpdateLastPackage(new TrackerDAL.Models.LastPackageUpdate
                                {
                                    Date = dhl.TrackingDetails[0].Date,
                                    Event = dhl.TrackingDetails[0].Event,
                                    PackageID = p.PackageID
                                });
                            }
                            if (dhl.TrackingDetails.Count > 0)
                            {
                                if (lastPackageUpdate.Date != null && lastPackageUpdate.Date != dhl.TrackingDetails[0].Date)
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
                            }

                            break;
                    }
                });
            }
        }

        private async Task SendPushNotification(int userID, string company, string companyLogo, string packageName, string packageEvent)
        {
            List<TrackerDAL.Models.Notification> notifications = await _system.GetUserNotifications(userID);
            if (notifications.Count > 0)
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
                    try
                    {
                        await _pushServiceClient.RequestPushMessageDeliveryAsync(pushSubscription, pushMessage);
                    }
                    catch (Exception ex)
                    {
                        await _system.AddLog("Notification: " + ex);
                        throw ex;
                    }
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
