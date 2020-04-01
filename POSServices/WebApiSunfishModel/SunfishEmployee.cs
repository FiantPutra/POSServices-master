using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebApiSunfishModel
{
    public class RESULT
    {
        public int POSITION_ID { get; set; }
        public string AREA { get; set; }
        public string STORE_CODE { get; set; }
        public int STATUS_ACTIVE { get; set; }
        public string POS_CODE { get; set; }
        public string EMPSTATUS { get; set; }
        public string END_DATE { get; set; }
        public string POS_NAME_EN { get; set; }
        public string NIK { get; set; }
        public string NAME { get; set; }
        public string STORE_NAME { get; set; }
        public string EMP_ID { get; set; }
        public string START_DATE { get; set; }
    }

    public class SunfishEmployee
    {
        public bool STATUS { get; set; }
        public string APPREQUEST { get; set; }
        public IList<RESULT> RESULT { get; set; }
    }
}
