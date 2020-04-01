using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class InforStore
    {
        public string CONO { get; set; }
        public string DIVI { get; set; }
        public string RSTN { get; set; }
        public string WHLO { get; set; }
        public string TX40 { get; set; }
        public string TOWN { get; set; }
        public string STCE { get; set; }
        public string CUNO { get; set; }


    }

    public class ResultStore
    {
        public string transaction { get; set; }
        public IList<InforStore> records { get; set; }
    }

    public class InforStoreAPI
    {
        public IList<ResultStore> results { get; set; }
        public bool wasTerminated { get; set; }
        public int nrOfSuccessfullTransactions { get; set; }
        public int nrOfFailedTransactions { get; set; }
    }
}
