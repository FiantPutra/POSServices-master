using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebAPIInforModel
{
    public class InforArticle
    {
        /*
        public string STAT { get; set; }
        public string ITNO { get; set; }
        public string ITDS { get; set; }
        public string FUDS { get; set; }
        public string DWNO { get; set; }
        public string RESP { get; set; }
        public string RENM { get; set; }
        public string UNMS { get; set; }
        public string DS01 { get; set; }
        public string ITGR { get; set; }
        public string DS02 { get; set; }
        public string ITCL { get; set; }
        public string DS03 { get; set; }
        public string BUAR { get; set; }
        public string DS04 { get; set; }
        public string ITTY { get; set; }
        public string DS05 { get; set; }
        public string TPCD { get; set; }
        public string MABU { get; set; }
        public string CHCD { get; set; }
        public string STCD { get; set; }
        public string BACD { get; set; }
        public string VOL3 { get; set; }
        public string NEWE { get; set; }
        public string GRWE { get; set; }
        public string PPUN { get; set; }
        public string DS06 { get; set; }
        public string BYPR { get; set; }
        public string WAPC { get; set; }
        public string QACD { get; set; }
        public string EPCD { get; set; }
        public string POCY { get; set; }
        public string ACTI { get; set; }
        public string HIE1 { get; set; }
        public string HIE2 { get; set; }
        public string HIE3 { get; set; }
        public string HIE4 { get; set; }
        public string HIE5 { get; set; }
        public string GRP1 { get; set; }
        public string GRP2 { get; set; }
        public string GRP3 { get; set; }
        public string GRP4 { get; set; }
        public string GRP5 { get; set; }
        public string CFI1 { get; set; }
        public string CFI2 { get; set; }
        public string CFI3 { get; set; }
        public string CFI4 { get; set; }
        public string CFI5 { get; set; }
        public string TXID { get; set; }
        public string ECMA { get; set; }
        public string PRGP { get; set; }
        public string DS07 { get; set; }
        public string INDI { get; set; }
        public string PUUN { get; set; }
        public string DS08 { get; set; }
        public string ALUC { get; set; }
        public string IEAA { get; set; }
        public string EXPD { get; set; }
        public string GRMT { get; set; }
        public string HAZI { get; set; }
        public string SALE { get; set; }
        public string TAXC { get; set; }
        public string DS09 { get; set; }
        public string ATMO { get; set; }
        public string ATMN { get; set; }
        public string TPLI { get; set; }
        public string FCU1 { get; set; }
        public string ALUN { get; set; }
        public string IACP { get; set; }
        public string HDPR { get; set; }
        public string AAD0 { get; set; }
        public string AAD1 { get; set; }
        public string CHCL { get; set; }
        public string ITRC { get; set; }
        public string VTCP { get; set; }
        public string DS10 { get; set; }
        public string VTCS { get; set; }
        public string DS11 { get; set; }
        public string LMDT { get; set; }
        public string DCCD { get; set; }
        public string PDCC { get; set; }
        public string SPUN { get; set; }
        public string CAWP { get; set; }
        public string CWUN { get; set; }
        public string CPUN { get; set; }
        public string ITRU { get; set; }
        public string TECR { get; set; }
        public string EXCA { get; set; }
        public string ACCG { get; set; }
        public string CCCM { get; set; }
        public string CCI1 { get; set; }
        public string CRI1 { get; set; }
        public string HVMT { get; set; }
        public string ITNE { get; set; }
        public string SPGV { get; set; }
        public string PDLN { get; set; }
        public string CPGR { get; set; }
        public string SUME { get; set; }
        public string SUMP { get; set; }
        public string EVGR { get; set; }
        public string QMGP { get; set; }
        public string POPN { set; get; }
        */
        public string ITNO { get; set; }
        public string ITDS { get; set; }
        public string BUAR { get; set; }
        public string DS04 { get; set; }
        public string FUDS { get; set; }
        public string ITGR { get; set; }
        public string DS02 { get; set; }
        public string ITCL { get; set; }
        public string DS03 { get; set; }
        public string POPN { get; set; }

        public string SIZE { get; set; }
        public string COLO { get; set; }
    }
    public class ResultArticle
    {
        public string transaction { get; set; }
        public IList<InforArticle> records { get; set; }
    }

    public class InforArticleAPI
    {
        public IList<ResultArticle> results { get; set; }
        public bool wasTerminated { get; set; }
        public int nrOfSuccessfullTransactions { get; set; }
        public int nrOfFailedTransactions { get; set; }
    }
}
