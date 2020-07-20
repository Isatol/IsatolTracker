using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TrackerAPI.Models;

namespace TrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private TrackerDAL.Tracking _tracking;
        public CompaniesController(Helper.ConnectionStrings connectionStrings)
        {
            _tracking = new TrackerDAL.Tracking(connectionStrings.GetConnectionString());
        }

        [HttpGet, Route("[action]")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _tracking.GetCompanies();
            return StatusCode(200, new 
            {
                companies
            });
        }
    }
}
