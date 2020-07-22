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
        private readonly TrackerDAL.TrackerSystem _trackerSystem;
        public TrackController(Isatol.Tracker.Track track, Helper.ConnectionStrings connectionStrings)
        {
            _track = track;
            _tracking = new TrackerDAL.Tracking(connectionStrings.GetConnectionString());
            _trackerSystem = new TrackerDAL.TrackerSystem(connectionStrings.GetConnectionString());
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetTrackingModel(int companyID, string trackingNumber)
        {
            try
            {                
                switch (companyID)
                {
                    case 1:
                        TrackingModel estafeta = await _track.EstafetaAsync(trackingNumber);
                        return StatusCode(200, new
                        {
                            trackingModel = estafeta
                        });
                    case 2:
                        TrackingModel fedex = await _track.FedexAsync(trackingNumber);
                        return StatusCode(200, new
                        {
                            trackingModel = fedex
                        });
                    case 3:
                        TrackingModel ups = await _track.UPSAsync(trackingNumber, Isatol.Tracker.Track.Locale.en_US);
                        return StatusCode(200, new
                        {
                            trackingModel = ups
                        });
                    default:
                        return StatusCode(200, new
                        {
                            message = "No company selected"
                        });
                }
            }
            catch (Exception ex)
            {                
                await _trackerSystem.AddLog(ex.ToString());
                return StatusCode(500, ex);
            }

        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetEstafeta(string trackingNumber)
        {
            try
            {
                TrackingModel trackingModel = await _track.EstafetaAsync(trackingNumber);
                return StatusCode(200, new
                {
                    trackingModel
                });
            }
            catch (Exception ex)
            {
                await _trackerSystem.AddLog(ex.ToString());
                return StatusCode(500, ex);
            }

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
                code = 1,
                message = $"Paquete agregado correctamente"
            });
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetUserPackages(int userID) 
        {
            List<TrackerDAL.Models.CompanyPackage> packages = await _tracking.GetUserPackages(userID);
            return StatusCode(200, new 
            {
                packages
            });
        }

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> DeletePackage([FromBody] TrackerDAL.Models.Package package)
        {
            if (!ModelState.IsValid) return StatusCode(400, ModelState);
            await _tracking.DeletePackage(package.PackageID);
            return StatusCode(200, new 
            {
                code = 1,
                messages = "Paquete eliminado"
            });
        }
    }
}
