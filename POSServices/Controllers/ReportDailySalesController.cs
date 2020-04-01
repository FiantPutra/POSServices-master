using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using POSServices.Data;
using POSServices.WebAPIModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POSServices.Controllers
{
    [Route("api/[controller]")]
    public class ReportDailySalesController : Controller
    {
        public int DAILY_SALES = 0;
        public int FORECAST_SALES = 1;
        public int STORE_TARGET = 2;
        public int REGIONAL_ACHIEVEMENT = 3;

        // GET api/<controller>/5
        [HttpGet]
        public IActionResult Get([FromQuery]int reportType, [FromQuery]string transDate)
        {
            int actualSalesValueResult = 0;
            int actualSalesQtyResult = 0;
            int actualSalesLastValueResult = 0;
            int actualSalesLastQtyResult = 0;
            int dailtGrowthValueResult = 0;
            int dailtGrowthQtyResult = 0;
            int dailyActualValueResult = 0;
            int focecastValueResult = 0;
            int focecastQtyResult = 0;
            int growthValueResult = 0;
            int growthQtyResult = 0;
            int targetQtyResult = 0;
            int targetValueResult = 0;

            List<ObjectAPIModel> listParam = new List<ObjectAPIModel>();
            List<Object> listModel = new List<Object>();
            DBSQLConnect connect = new DBSQLConnect();

            var inputDate = new ObjectAPIModel();
            inputDate.param = "@transDate";
            inputDate.typeValue = DbType.String;
            inputDate.value = transDate;
            listParam.Add(inputDate);

            try
            {
                if (reportType == DAILY_SALES)
                {
                    connect.sqlCon().Open();
                    string cmd = "select top 100[StoreType],  SUM(qty) as actualSalesQty,  SUM(TotalAmounTransaction) as actualSalesValue, " +
                                 "(select SUM(Qty) from vTransactionStore where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and day(TransactionDate) = day(@transDate)) as actualSalesLastQty, " +
                                 "(select SUM(TotalAmounTransaction) from vTransactionStore where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and day(TransactionDate) = day(@transDate)) as actualSalesLastValue," +
                                 "((SUM(qty) - (select SUM(Qty) from vTransactionStore where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and day(TransactionDate) = day(@transDate)))/ (select SUM(Qty) from vTransactionStore where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and day(TransactionDate) = day(@transDate))) *100 as dailtGrowthQty ," +
                                 "((SUM(TotalAmounTransaction) - (select SUM(TotalAmounTransaction) from vTransactionStore where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and day(TransactionDate) = day(@transDate)))/ (select SUM(TotalAmounTransaction) from vTransactionStore where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and day(TransactionDate) = day(@transDate))) *100 as dailtGrowthValue," +
                                 "(select sum(Target) from StoreTarget )  / SUM(TotalAmounTransaction)  as dailyActualValue " +
                                 " from vTransactionStore vts where Month(TransactionDate) = Month(@transDate) group by [StoreType] ";

                    connect.sqlDataRd = connect.ExecuteDataReaderWithParams(cmd, connect.sqlCon(), listParam);

                    if (connect.sqlDataRd.HasRows)
                    {

                        while (connect.sqlDataRd.Read())
                        {
                            int.TryParse(connect.sqlDataRd["actualSalesQty"].ToString(), out actualSalesQtyResult);
                            int.TryParse(connect.sqlDataRd["actualSalesValue"].ToString(), out actualSalesValueResult);
                            int.TryParse(connect.sqlDataRd["actualSalesLastValue"].ToString(), out actualSalesLastValueResult);
                            int.TryParse(connect.sqlDataRd["actualSalesLastQty"].ToString(), out actualSalesLastQtyResult);
                            int.TryParse(connect.sqlDataRd["dailtGrowthValue"].ToString(), out dailtGrowthValueResult);
                            int.TryParse(connect.sqlDataRd["dailtGrowthQty"].ToString(), out dailtGrowthQtyResult);
                            int.TryParse(connect.sqlDataRd["dailyActualValue"].ToString(), out dailyActualValueResult);

                            var model = new
                            {
                                storeType = connect.sqlDataRd["StoreType"].ToString(),
                                actualSalesValue = actualSalesValueResult,
                                actualSalesQty = actualSalesQtyResult,
                                actualSalesLastValue = actualSalesLastValueResult,
                                actualSalesLastQty = actualSalesLastQtyResult,
                                dailtGrowthValue = dailtGrowthValueResult,
                                dailtGrowthQty = dailtGrowthQtyResult,
                                dailyActualValue = dailyActualValueResult,
                                dailyActualQty = 0,
                                dailyActualCompValue = 0,
                                dailyActualCompQty = 0
                            };

                            listModel.Add(model);
                        }
                    }
                }
                else
                if (reportType == FORECAST_SALES)
                {
                    connect.sqlCon().Open();
                    string cmd = "select top 100[StoreType], " +
                                 "((select SUM(Qty) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate)) and month(TransactionDate) = month(@transDate) and vtsi.StoreType = vts.StoreType)/ day(@transDate))*31 as focecastQty, " +
                                 "((select SUM(TotalAmounTransaction) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate)) and month(TransactionDate) = month(@transDate) and vtsi.StoreType = vts.StoreType)/ day(@transDate))*31 as focecastValue, " +
                                 "(select SUM(Qty) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and vtsi.StoreType = vts.StoreType)  as actualSalesQty, " +
                                 "(select SUM(TotalAmounTransaction) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate) - 1) and month(TransactionDate) = month(@transDate) and vtsi.StoreType = vts.StoreType) as actualSalesValue " +
                                 "from vTransactionStore vts where Month(TransactionDate) = Month(@transDate) group by [StoreType]"; ;

                    connect.sqlDataRd = connect.ExecuteDataReaderWithParams(cmd, connect.sqlCon(), listParam);

                    if (connect.sqlDataRd.HasRows)
                    {
                        while (connect.sqlDataRd.Read())
                        {
                            int.TryParse(connect.sqlDataRd["actualSalesValue"].ToString(), out actualSalesValueResult);
                            int.TryParse(connect.sqlDataRd["actualSalesQty"].ToString(), out actualSalesQtyResult);
                            int.TryParse(connect.sqlDataRd["focecastValue"].ToString(), out focecastValueResult);
                            int.TryParse(connect.sqlDataRd["focecastQty"].ToString(), out focecastQtyResult);
                            int.TryParse(connect.sqlDataRd["growthValue"].ToString(), out growthValueResult);
                            int.TryParse(connect.sqlDataRd["growthQty"].ToString(), out growthQtyResult);

                            var model = new
                            {
                                storeType = connect.sqlDataRd["StoreType"].ToString(),
                                actualSalesValue = actualSalesValueResult,// connect.sqlDataRd["actualSalesValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["actualSalesValue"].ToString()),
                                actualSalesQty = actualSalesQtyResult,// connect.sqlDataRd["actualSalesQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["actualSalesQty"].ToString()),
                                focecastValue = focecastValueResult,//connect.sqlDataRd["focecastValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["focecastValue"].ToString()),
                                focecastQty = focecastQtyResult, //connect.sqlDataRd["focecastQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["focecastQty"].ToString()),
                                growthValue = growthValueResult, // connect.sqlDataRd["growthValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["growthValue"].ToString()),
                                growthQty = growthQtyResult //connect.sqlDataRd["growthQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["growthQty"].ToString())
                            };
                        }
                    }
                }
                else
                if (reportType == STORE_TARGET)
                {
                    connect.sqlCon().Open();
                    string cmd = "select top 100 [StoreType], " +
                                 "(select sum(TargetQty) from VStoreTarget st where st.StoreType = vts.StoreType) as targetQty, " +
                                 "(select sum(target) from VStoreTarget st where st.StoreType = vts.StoreType) as targetValue, " +
                                 "((select SUM(Qty) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate)) and month(TransactionDate) = month(@transDate) and vtsi.StoreType = vts.StoreType)/ day(@transDate))*31 as focecastValue, " +
                                 "((select SUM(TotalAmounTransaction) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate)) and month(TransactionDate) = month(@transDate) and vtsi.StoreType = vts.StoreType)/ day(@transDate))*31 as focecastValue " +
                                " from vTransactionStore vts where Month(TransactionDate) = Month(@transDate) group by [StoreType]";

                    connect.sqlDataRd = connect.ExecuteDataReaderWithParams(cmd, connect.sqlCon(), listParam);

                    if (connect.sqlDataRd.HasRows)
                    {
                        while (connect.sqlDataRd.Read())
                        {
                            int.TryParse(connect.sqlDataRd["targetQty"].ToString(), out targetQtyResult);
                            int.TryParse(connect.sqlDataRd["targetValue"].ToString(), out targetValueResult);
                            int.TryParse(connect.sqlDataRd["focecastValue"].ToString(), out focecastValueResult);
                            int.TryParse(connect.sqlDataRd["focecastQty"].ToString(), out focecastQtyResult);

                            var model = new
                            {
                                storeType = connect.sqlDataRd["StoreType"].ToString(),
                                targetQty = targetQtyResult, // connect.sqlDataRd["targetQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["targetQty"].ToString()),
                                targetValue = targetValueResult, //connect.sqlDataRd["targetValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["targetValue"].ToString()),
                                focecastValue = focecastValueResult, //connect.sqlDataRd["focecastValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["focecastValue"].ToString()),
                                focecastQty = focecastQtyResult //connect.sqlDataRd["focecastQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["focecastQty"].ToString())
                            };
                        }
                    }
                }
                else
                if (reportType == REGIONAL_ACHIEVEMENT)
                {
                    connect.sqlCon().Open();
                    string cmd = "select top 100 [regional] as regional,  " +
                                 " SUM(qty) as actualSalesQty," +
                                 " SUM(TotalAmounTransaction)'Actual Sales Value', " +
                                 " (select sum(TargetQty) from VStoreTarget st where st.regional = vts.regional) as targetQty, " +
                                 " (select sum(target) from VStoreTarget st where st.regional = vts.regional) as targetValue, " +
                                 " ((select SUM(Qty) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate)) and month(TransactionDate) = month(@transDate) and vtsi.regional = vts.regional)/ day(@transDate))*31 as focecastQty," +
                                 " ((select SUM(TotalAmounTransaction) from vTransactionStore vtsi where (year(TransactionDate) = year(@transDate)) and month(TransactionDate) = month(@transDate) and vtsi.regional = vts.regional)/ day(@transDate))*31 as focecastValue " +
                                " from vTransactionStore vts where Month(TransactionDate) = Month(@transDate)  and regional is not null group by[regional]";

                    connect.sqlDataRd = connect.ExecuteDataReaderWithParams(cmd, connect.sqlCon(), listParam);

                    if (connect.sqlDataRd.HasRows)
                    {
                        while (connect.sqlDataRd.Read())
                        {
                            int.TryParse(connect.sqlDataRd["actualSalesValue"].ToString(), out actualSalesValueResult);
                            int.TryParse(connect.sqlDataRd["actualSalesQty"].ToString(), out actualSalesQtyResult);
                            int.TryParse(connect.sqlDataRd["targetQty"].ToString(), out targetQtyResult);
                            int.TryParse(connect.sqlDataRd["targetValue"].ToString(), out targetValueResult);
                            int.TryParse(connect.sqlDataRd["focecastValue"].ToString(), out focecastValueResult);
                            int.TryParse(connect.sqlDataRd["focecastQty"].ToString(), out focecastQtyResult);

                            var model = new
                            {
                                regional = connect.sqlDataRd["regional"].ToString(),
                                actualSalesQty = actualSalesQtyResult, //connect.sqlDataRd["actualSalesQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["actualSalesQty"].ToString()),
                                actualSalesValue = actualSalesValueResult, //connect.sqlDataRd["actualSalesValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["actualSalesValue"].ToString()),
                                targetQty = targetQtyResult, //connect.sqlDataRd["targetQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["targetQty"].ToString()),
                                targetValue = targetValueResult, //connect.sqlDataRd["targetValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["targetValue"].ToString()),
                                focecastValue = focecastValueResult, //connect.sqlDataRd["focecastValue"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["focecastValue"].ToString()),
                                focecastQty = focecastQtyResult //connect.sqlDataRd["focecastQty"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["focecastQty"].ToString())
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            finally
            {
                if (connect.sqlDataRd != null)
                    connect.sqlDataRd.Close();

                if (connect.sqlCon().State == ConnectionState.Open)
                    connect.sqlCon().Close();
            }

            return Ok(listModel);
        }


    }
}
