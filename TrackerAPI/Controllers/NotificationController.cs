using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrackerAPI.Helper;
using TrackerAPI.Models;

namespace TrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly TrackerDAL.TrackerSystem _system;
        private Models.PushNotification _pushNotification;
        public NotificationController(Helper.ConnectionStrings connectionStrings, IOptions<Models.PushNotification> pushNotification)
        {
            _system = new TrackerDAL.TrackerSystem(connectionStrings.GetConnectionString());
            _pushNotification = pushNotification.Value;
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetVapidPublicKey()
        {
            return StatusCode(200, new
            {
                publicKey = _pushNotification.PublicKey
            });
        }
        
        [HttpPost, Route("[action]")]
        public async Task<IActionResult> AddNotificationSubscription([FromBody] NotificationSubscription notification)
        {
            string keys = Newtonsoft.Json.JsonConvert.SerializeObject(notification.PushSubscription.Keys, Newtonsoft.Json.Formatting.Indented);
            await _system.InsertNotification(new TrackerDAL.Models.Notification 
            {
                Keys = keys,
                Endpoint = notification.PushSubscription.Endpoint,
                UsersID = notification.UserID
            });
            return StatusCode(200, new 
            {
                message = "Subscription Added"
            });
        }
    }
}
