using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class RecordPettyCash
    {

        public string CONO { get; set; }
        public string DIVI { get; set; }
        public string XRCD { get; set; }
        public string ITRN { get; set; }
        public string WHLO { get; set; }
        public string ORNO { get; set; }
        public string DLIX { get; set; }
        public string PONR { get; set; }
        public string POSX { get; set; }
        public string CUCD { get; set; }
        public string CUNO { get; set; }
        public string ITNO { get; set; }
        public string TRDT { get; set; }
        public string TRTM { get; set; }
        public string CUAM { get; set; }
        public string VTCD { get; set; }
        public string PYCD { get; set; }
        public string ALUN { get; set; }
        public string IVQA { get; set; }
        public string INYR { get; set; }
        public string REFE { get; set; }
        public string VTP1 { get; set; }
        public string ARAT { get; set; }
        public string CRTP { get; set; }
        public string ACDT { get; set; }
        public string CSHC { get; set; }
    }

    public class TransactionPettyCash
    {
        public string transaction { get; set; }
        public RecordPettyCash record { get; set; }
    }

    public class InforPettyCash
    {
        public string program { get; set; }
        public IList<TransactionPettyCash> transactions { get; set; }
    }
}
