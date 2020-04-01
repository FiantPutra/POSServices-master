using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class RecordDeliveryReciepts
    {
        public string PRFL { get; set; }
        public string WHLO { get; set; }
        public string E0PA { get; set; }
        public string E065 { get; set; }
        public string TWHL { get; set; }
        public string ITNO { get; set; }
        public string WHSL { get; set; }
        public string QTY { get; set; }
        public string RIDN { get; set; }
        public string RIDL { get; set; }
        public string RIDI { get; set; }
        public string PACN { get; set; }
    }

    public class TransactionDeliveryReciepts
    {
        public string transaction { get; set; }
        public RecordDeliveryReciepts record { get; set; }
    }

    public class InforDeliveryReciepts
    {
        public string program { get; set; }
        public IList<TransactionDeliveryReciepts> transactions { get; set; }
    }
}
