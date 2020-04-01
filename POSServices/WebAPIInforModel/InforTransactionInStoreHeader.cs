using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class RecordInStore
    {
        public string CONO { get; set; }
        public string CUNO { get; set; }
        public string ORTP { get; set; }
        public string FACI { get; set; }
        public string MODL { get; set; }
        public string TEDL { get; set; }
        public string CUOR { get; set; }
        public string ORDT { get; set; }
        public string CUCD { get; set; }
        public string CRTP { get; set; }
        public string CUDT { get; set; }
        public string RLDT { get; set; }
        public string RLDZ { get; set; }
    }

    public class TransactionInStore
    {
        public string transaction { get; set; }
        public RecordInStore record { get; set; }
    }

    public class InforTransactionInStoreHeader
    {
        public string program { get; set; }
        public IList<TransactionInStore> transactions { get; set; }
    }

    //for result
    public class RecordGetOrno
    {
        public string ORNO { get; set; }
    }

    public class ResultGetOrno
    {
        public string transaction { get; set; }
        public string errorMessage { get; set; }

        public IList<RecordGetOrno> records { get; set; }
    }

    public class InforTransactionInStoreResult
    {
        public IList<ResultGetOrno> results { get; set; }
        public bool wasTerminated { get; set; }
        public string terminationErrorType { get; set; }
        public string terminationReason { get; set; }
        public int nrOfSuccessfullTransactions { get; set; }
        public int nrOfFailedTransactions { get; set; }
    }
    //end of result

    //for transaction in store lines
    public class RecordTransactionInStoreLine
    {
        public string POPN { get; set; }
        public string CONO { get; set; }
        public string ORNO { get; set; }
        public string ITNO { get; set; }
        public string ORQT { get; set; }
        public string WHLO { get; set; }
        public string SAPR { get; set; }
        public string DWDT { get; set; }
        public string DWDZ { get; set; }
        public string DWHZ { get; set; }
        public string CUOR { get; set; }
        public string SACD { get; set; }
        public string SPUN { get; set; }
        public string EDFP { get; set; }
        public string LTYP { get; set; }
        public string CTYP { get; set; }
        public string DIA1 { get; set; }
        public string DIA2 { get; set; }
        public string DIA3 { get; set; }
        public string DIA4 { get; set; }
        public string DIA5 { get; set; }
        public string DIA6 { get; set; }
        public string WHSL { get; set; }

        //add by frank
        //30 oktober 2019
        //request by hendra tjoa for employee OIS
        public string TEPY { get; set; }

    }

    public class TransactionInStoreLine
    {
        public string transaction { get; set; }
        public RecordTransactionInStoreLine record { get; set; }
    }

    public class InforTransactionInStoreLine
    {
        public string program { get; set; }
        public IList<TransactionInStoreLine> transactions { get; set; }
    }
}
