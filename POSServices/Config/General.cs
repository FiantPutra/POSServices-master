using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.Config
{
    public static class General
    {
        //production
        public static String urlInfor = "https://m3.biensicore.co.id:56756/m3api-rest/v2/execute";// infor prod
        public static String getInfor = "https://m3.biensicore.co.id/M3Uploader";

        //testing
        //    public static String urlInfor = "https://m3.biensicore.co.id:21254/m3api-rest/v2/execute";// infor prod
        //    public static String getInfor = "https://m3.biensicore.co.id/M3UploaderTST";

        //public static String sqlRoute = @"Data Source=192.168.1.6\MPOSDB;Initial Catalog=db_biensi_azz_prod;Trusted_Connection=True;Integrated Security=False;user ID= saa;Password= password123!";

        //public static String sqlRoute = @"Data Source=10.5.50.41;Initial Catalog=DB_BIENSI_POS;Trusted_Connection=True;Integrated Security=False;user ID=biensi;Password=p@$$word";
        public static String sqlRoute = @"Server=10.5.50.41; Database=DB_BIENSI_POS; User Id=biensi; Password=p@$$w0rd; Trusted_Connection=False; MultipleActiveResultSets=true";

        //public static String sqlRoute = @"Server=SERVER-VSU; Database=DB_BIENSI_POS; User Id=WCS\Administrator; Password=ber217an; Trusted_Connection=False; MultipleActiveResultSets=true";


        public static String numberSequenceDO(int id)
        {
            String doNumber = "";
            doNumber = "DO-" + DateTime.Now.Year + "" + DateTime.Now.Month.ToString("d2") + id;
            return doNumber;

        }

        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }
            return String.Empty;
        }

        public static decimal percentageValue(decimal value, decimal pembagi)
        {

            return value / pembagi;
        }


    }
}
