using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Models
{
    public class DownloadFileModel
    {
        public int TaskId { get; set; }
        
        public int ItemId { get; set; }

        public string FileName { get; set; }

    }
}
