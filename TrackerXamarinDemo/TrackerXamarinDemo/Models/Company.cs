using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace TrackerXamarinDemo.Models
{
    public class CompanyList
    {
        public List<Company> Companies { get; set; }
    }

    public class Company
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public ImageSource LogoImageSource
        {
            get
            {
                string[] base64Logo = this.Logo.Split(',');
                string imgData = base64Logo[1];
                byte[] imgBytes = Convert.FromBase64String(imgData);
                return ImageSource.FromStream(() => new MemoryStream(imgBytes));
            }
        }
    }    
}
