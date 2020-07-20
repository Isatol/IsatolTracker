using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Isatol.Tracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly Isatol.Tracker.Track _track;
        private readonly TrackerDAL.Tracking _tracking;
        public TrackController(Isatol.Tracker.Track track, Helper.ConnectionStrings connectionStrings)
        {
            _track = track;
            _tracking = new TrackerDAL.Tracking(connectionStrings.GetConnectionString());
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetEstafeta(string trackingNumber)
        {            
            TrackingModel trackingModel = await _track.EstafetaAsync(trackingNumber);
            return StatusCode(200, new 
            {
                trackingModel
            });
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetFedex(string trackingNumber)
        {
            TrackingModel trackingModel = await _track.FedexAsync(trackingNumber);
            return StatusCode(200, new 
            {
                trackingModel
            });
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetUPS(string trackingNumber)
        {
            TrackingModel trackingModel = await _track.UPSAsync(trackingNumber, Isatol.Tracker.Track.Locale.es_MX);
            return StatusCode(200, new 
            {
                trackingModel
            });
        }

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> InsertPackage([FromBody] TrackerDAL.Models.Package package)
        {
            if (!ModelState.IsValid) return StatusCode(400, ModelState);
            await _tracking.InsertPackage(package);
            return StatusCode(200, new 
            {
                message = $"Package added"
            });
        }
    }
}
