using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class ApplicationVersion
    {
        public int Id { get; set; }
        public string AppType { get; set; }
        public string Version { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string ReasonUpdate { get; set; }
        public string UrlDownload { get; set; }
    }
}
