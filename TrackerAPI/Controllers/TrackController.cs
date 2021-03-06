﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Isatol.Tracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrackerDAL.Models;

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
                TrackingModel trackingModel = await GetPackages(companyID, trackingNumber);
                return StatusCode(200, new
                {
                    trackingModel = trackingModel
                });
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
            var packages = await GetPackages(package.CompanyID, package.TrackingNumber);
            LastPackageUpdate lastPackageUpdate = new LastPackageUpdate();
            if (packages.TrackingDetails.Count > 0) 
            {
                lastPackageUpdate.Date = packages.TrackingDetails[0].Date;
                lastPackageUpdate.Event = packages.TrackingDetails[0].Event;
            }                
            await _tracking.InsertPackage(package, lastPackageUpdate);
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

        public async Task<TrackingModel> GetPackages(int companyID, string trackingNumber)
        {
            switch (companyID)
            {
                case 1:
                    TrackingModel estafeta = await _track.EstafetaAsync(trackingNumber);
                    return estafeta;
                case 2:
                    TrackingModel fedex = await _track.FedexAsync(trackingNumber);
                    return fedex;
                case 3:
                    TrackingModel ups = await _track.UPSAsync(trackingNumber, Isatol.Tracker.Track.Locale.es_MX);
                    return ups;
                case 4:
                    TrackingModel dhl = await _track.DHLAsync(trackingNumber, Isatol.Tracker.Track.Locale.es_MX);
                    return dhl;
                default:
                    return default;
            }
        }
    }
}
