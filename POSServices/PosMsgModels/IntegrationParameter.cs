using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class IntegrationParameter
    {
        public string Erpdatabase { get; set; }
        public string BackendPosdatabase { get; set; }
        public int Recid { get; set; }
    }
}
