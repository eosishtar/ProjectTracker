using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class EmailModel
    {
        public string[] ToEmails { get; set; }
        public string[] CCEmails { get; set; }
        public string[] BCCEmails { get; set; }
        public string[] Attachments { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool SendAsHtml { get; set; } = false;
    }
}
