using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.WebApiSunfishModel
{
    public class RESULTPosition
    {
        public int POSITION_ID { get; set; }
        public string POSITION_NAME { get; set; }
        public string POSITION_CODE { get; set; }
        public string POS_FLAG { get; set; }
    }

    public class SunfishPosition
    {
        public bool STATUS { get; set; }
        public string APPREQUEST { get; set; }
        public IList<RESULTPosition> RESULT { get; set; }
    }
}
