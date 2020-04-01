using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class TableToSynch
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Sqlcommand { get; set; }
        public int GeneratePerstore { get; set; }
        public int RecordLoadLimit { get; set; }
        public int Dataperstore { get; set; }
        public int Status { get; set; }
        public string TablePrimarykey { get; set; }
    }
}
