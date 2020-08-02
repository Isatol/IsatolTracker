using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerXamarinDemo.Models
{
    [Table("User")]
    public class User
    {        
        [PrimaryKey]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime ExpToken { get; set; }
    }
}
