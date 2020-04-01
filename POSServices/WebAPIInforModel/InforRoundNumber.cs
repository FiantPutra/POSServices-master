using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class RecordRoundNumber
    {
        public string ITRN { get; set; }
    }
    public class ResultRoundNumber
    {
        public string transaction { get; set; }
        public IList<RecordRoundNumber> records { get; set; }
    }

    public class InforRoundNumber
    {
        public IList<ResultRoundNumber> results { get; set; }
        public bool wasTerminated { get; set; }
        public int nrOfSuccessfullTransactions { get; set; }
        public int nrOfFailedTransactions { get; set; }
    }
}
