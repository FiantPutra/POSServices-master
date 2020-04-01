using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class InforWarehouse
    {
        public string CONO { get; set; }
        public string DIVI { get; set; }
        public string FACI { get; set; }
        public string WHLO { get; set; }
        public string WHNM { get; set; }
        public string STOF { get; set; }
        public string DCIN { get; set; }
        public string RSTN { get; set; }
        public string PHNO { get; set; }
        public string CONM { get; set; }
        public string ADR1 { get; set; }
        public string ADR2 { get; set; }
        public string ADR3 { get; set; }
        public string ADR4 { get; set; }
        public string CSCD { get; set; }
        public string PONO { get; set; }
        public string ECAR { get; set; }
        public string TOWN { get; set; }
        public string PNOD { get; set; }
        public string GEOX { get; set; }
        public string GEOY { get; set; }
        public string GEOZ { get; set; }
        public string AGEX { get; set; }
        public string AGEY { get; set; }
        public string AGEZ { get; set; }
        public string TIZO { get; set; }
        public string T40X { get; set; }
        public string T15X { get; set; }
        public string TGMT { get; set; }
        public string DLST { get; set; }
        public string VFDT { get; set; }
        public string VTDT { get; set; }
        public string MAWH { get; set; }
        public string PWGR { get; set; }
        public string FRCO { get; set; }
        public string WHTY { get; set; }
        public string MWHL { get; set; }
        public string RDIL { get; set; }
    }

    public class ResultWarehouse
    {
        public string transaction { get; set; }
        public IList<InforWarehouse> records { get; set; }
    }

    public class InforWarehouseAPI
    {
        public IList<ResultWarehouse> results { get; set; }
        public bool wasTerminated { get; set; }
        public int nrOfSuccessfullTransactions { get; set; }
        public int nrOfFailedTransactions { get; set; }
    }
}
