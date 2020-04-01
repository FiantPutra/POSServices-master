using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class RecordReturn
    {
        public string CONO { get; set; }
        public string MSGN { get; set; }
    }

    public class Result
    {
        public string transaction { get; set; }
        public IList<RecordReturn> records { get; set; }
        public string errorMessage { get; set; }
        public string errorType { get; set; }
        public string errorCode { get; set; }
        public string errorCfg { get; set; }
        public string errorField { get; set; }


    }

    public class InforObjPostReturn
    {
        public IList<Result> results { get; set; }
        public bool wasTerminated { get; set; }
        public int nrOfSuccessfullTransactions { get; set; }
        public int nrOfFailedTransactions { get; set; }
    }
}
