using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerAPI.Models
{
    public class EmailSettings
    {
        /// <summary>
        /// SMTP address
        /// </summary>
        public string SMTPServer { get; set; }
        /// <summary>
        /// Number Port
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// EMail adress
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Indicates if SSL will be used
        /// </summary>
        public bool UseSSL { get; set; }
        /// <summary>
        /// Mail title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the email body. Use this also for the content of a template
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the mail message body is in Html.
        /// </summary>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// Mail subject
        /// </summary>
        public string Subject { get; set; }
    }
}
