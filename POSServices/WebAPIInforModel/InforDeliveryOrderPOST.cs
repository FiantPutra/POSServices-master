using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class RecordDeliveryPOST
    {
        public string PRMD { get; set; }
        public string CONO { get; set; }
        public string WHLO { get; set; }
        public string MSGN { get; set; }
        public string PACN { get; set; }
        public string GEDT { get; set; }
        public string GETM { get; set; }
        public string E0PA { get; set; }
        public string E0PB { get; set; }
        public string E065 { get; set; }
        public string CUNO { get; set; }
        public string ADID { get; set; }
        public string ITNO { get; set; }
        public string POPN { get; set; }
        public string ALWQ { get; set; }
        public string ALWT { get; set; }
        public string WHSL { get; set; }
        public string TWSL { get; set; }
        public string BANO { get; set; }
        public string CAMU { get; set; }
        public string ALQT { get; set; }
        public string DLQT { get; set; }
        public string RIDN { get; set; }
        public string RIDL { get; set; }
        public string RIDX { get; set; }
        public string RIDI { get; set; }
        public string PLSX { get; set; }
        public string DLIX { get; set; }
        public string USD1 { get; set; }
        public string USD2 { get; set; }
        public string USD3 { get; set; }
        public string USD4 { get; set; }
        public string USD5 { get; set; }
        public string CAWE { get; set; }
        public string RTDT { get; set; }
        public string TRTP { get; set; }
        public string RESP { get; set; }
        public string PMSN { get; set; }
        public string OPNO { get; set; }
        public string RSCD { get; set; }
        public string RORC { get; set; }
        public string RORN { get; set; }
        public string RORL { get; set; }
        public string RORX { get; set; }
        public string BREF { get; set; }
        public string BRE2 { get; set; }
        public string RPDT { get; set; }
        public string RPTM { get; set; }
        public string REPN { get; set; }
        public string TWHL { get; set; }
        public string QTY { get; set; }


    }

    public class TransactionDeliveryPOST
    {
        public string transaction { get; set; }
        public RecordDeliveryPOST record { get; set; }
    }

    public class InforDeliveryOrderPOST
    {
        public string program { get; set; }
        public IList<TransactionDeliveryPOST> transactions { get; set; }
    }
}
