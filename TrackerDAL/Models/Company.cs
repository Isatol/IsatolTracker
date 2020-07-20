using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerDAL.Models
{
    public class Company
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string ValidRegex { get; set; }
    }
}
