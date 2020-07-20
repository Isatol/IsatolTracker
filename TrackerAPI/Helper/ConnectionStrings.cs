using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerAPI.Helper
{
    public class ConnectionStrings
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        public ConnectionStrings(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _environment.IsDevelopment() ? _configuration.GetConnectionString("Local") : _configuration.GetConnectionString("Server");
        }
    }
}
