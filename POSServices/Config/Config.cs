using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.Config
{
    public class RetailEnum
    {
        public static int discountNormal = 0;
        public static int discountEmployee = 1;
        public static int discountMixAndMatch = 2;
        public static int discountBuyAndGet = 3;

        public static int requestTransaction = 0;
        public static int mutasiTransaction = 1;
        public static int returnTransaction = 2;
        public static int doTransaction = 3;
        public static int hoTransaction = 66;

        public static int doStatusPending = 0;
        public static int doStatusConfirmed = 1;

        /*
        1=DeliveryOrder
        2=Mutasi Order
        3=Return Irder
        4=Sales Transaction
        9=Beginning Balance
        */
        public static int DeliveryOrder = 0;
        public static int MutasiOrderOut = 1;
        public static int MutasiOrderIn = 4;
        public static int MutasiOrderReject = 5;
        public static int ReturnOrder = 2;
        public static int SalesTransaction = 3;
        public static int BeginningBalance = 9;
        public static int HOTransaction = 66;

        public static int transactionStore = 1;
        public static int transactionStoreinStore = 2;
        public static int transactionEmployee = 3;

        //for do type
        public static String RequestOrderEnum = "AD6";
        public static String ReturnOrderEnum = "RTT"; //AD9
        public static String MutasiOrderEnum = "AD0";

        // for store in store
        public static String TransactionStoreInStore = "A02";

        //        public static String connectionStrings = @"Data Source=192.168.1.6\DBMPOS;Initial Catalog=db_biensi_azz_prod;Integrated Security=True";
        public static String connectionStrings = @"Data Source=SERVER-VSU;Initial Catalog=DB_BIENSI_POS;Integrated Security=True";

        public static int expenseBudget = 0;
        public static int expenseExpense = 1;        
    }

    public class sunfishConfig
    {        
        public static string username = "biensi";
        public static string appname = "multiple apps";
        public static string RSAkey = "MIIDLjCCAhYCAQAwgegxCzAJBgNVBAYTAklEMRAwDgYDVQQIDAdCYW5kdW5nMVUwUwYDVQQHDExKYWxhbiBDaW1pbmNyYW5nIE5vIDJiIFJUIDAwMyBSVyAwMDMgS2VsdXJhaGFuIENpbWluY3JhbmcgS2VjYW1hdGFuIEdlZGViYWdlMR8wHQYDVQQKDBZDVi4gQmktZW5zaSBGZXN5ZW5pbmRvMRYwFAYDVQQLDA1tdWx0aXBsZSBhcHBzMQ8wDQYDVQQDDAZiaWVuc2kxJjAkBgkqhkiG9w0BCQEWF3l1c3VwLmZyZWRkaUBkYXRhb24uY29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApcrWbXVUOCzvB+bs0kRjKw7LoWtLTN601J/yGizYMSLYhm/FNbWBzfaDhdlqpplYWkGUB3ZQ07fmFGKiOJUhSX3MB0lYWKQL88jHivnWG0mprI+/vhOxbJj5bb3i0aCFoz1kKm+P42cV6m5V+hyrW8qcsf8YBrY74Dhfie3ZthTEgKfUyXYV6YiVJUmLQdwg3ltFzwKDLWba/DQa3MtKfi3zbzGXN7cdmC9ajsktTOG3REOy5+ln0VzorqPYnAlUMKCb4mZjrKqmLZzmIgIpNQn6Yb3FsD5WOSzgDT7GQlFx+6W1gqstZquE5RR3qYfAMhywQ3OsdjBFZ7TV16K27QIDAQABoAAwDQYJKoZIhvcNAQELBQADggEBADCxkH/jI4o8DqsC9X5gw+54CTpvVDX9aiqJ/o/jk9ONHOOqz/BhJMnrGfOCdIKuFsk/4yFTNsqGkGdoeWhid1s7bhhAM7uuyNHx03Pvnid5B+fuEMUe60SXz83o21K8H7KCVkTWRjH8X+TtqgLf/vpehUp0UbWMsun5nDVJPOF3fdasMwLvcoelGAtOuVhBdO4PaSuLhPMYj5Bfz/xDxh5of5IU9vTeycvD++UQQIJoq7cOsJmC7EmEavWd3qWxnAdoH8k89L1kotkJBEqka7LM5Po8mQIBnaZAj9tfw99PL+w93C6b4TNM1xOXUdkqxaAiyTwXcbZKvDjDm+9o33o=";
        public static string account = "biensi";
        public static string Uri = "http://117.54.122.20/sfapi/";
    }

    public class inforConfig
    {
        public static string username = "biensicore\\infor6";
        public static string password = "Daksa007!";
    }
}
