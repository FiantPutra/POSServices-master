using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class Record
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
        public string DUDT { get; set; }
        public string PYCD { get; set; }
        public string ALUN { get; set; }
        public string IVQA { get; set; }
        public string DIA1 { get; set; }
        public string DIA2 { get; set; }
        public string DIA3 { get; set; }
        public string DIA4 { get; set; }
        public string DIA5 { get; set; }
        public string DIA6 { get; set; }
        public string IVNO { get; set; }
        public string INYR { get; set; }
        public string VTP1 { get; set; }
        public string ARAT { get; set; }
        public string CRTP { get; set; }
        public string CSHC { get; set; }
        public string ACDT { get; set; }
        public string OPMM { get; set; }
        public string OPSS { get; set; }
        public string OPGL { get; set; }


        //add for sales ticket pay
        //  public string FACI { get; set; }
    }

    public class Transaction
    {
        public string transaction { get; set; }
        public Record record { get; set; }
    }

    public class InforObjPost
    {
        public string program { get; set; }
        public IList<Transaction> transactions { get; set; }
    }
}
