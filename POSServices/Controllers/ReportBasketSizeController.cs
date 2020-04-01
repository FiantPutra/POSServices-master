using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using POSServices.Data;
using POSServices.Models;
using POSServices.WebAPIModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace POSServices.Controllers
{
    [Route("api/[controller]")]
    public class ReportBasketSizeController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        public ReportBasketSizeController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get([FromQuery]string transDate)
        {
            BasketSizeAPIModel model;
            List<BasketSizeAPIModel> listModel = new List<BasketSizeAPIModel>();
            List<ObjectAPIModel> listParam = new List<ObjectAPIModel>();

            DBSQLConnect connect = new DBSQLConnect();

            try
            {
                connect.sqlCon().Open();
                string cmd = "select tr.StoreCode as Code, s.Name as Name, TransactionMin15 = (select count(*) from [Transaction] where day(TransactionDate) <= 15 and StoreCode = tr.StoreCode and Month(TransactionDate) = MONTH(@InputDate) and YEAR(TransactionDate) = YEAR(@InputDate))," +
                             "QtyMin15 = (select sum(Qty) from [Transaction] where day(TransactionDate) <= 15 and StoreCode = tr.StoreCode and Month(TransactionDate) = MONTH(@InputDate) and YEAR(TransactionDate) = YEAR(@InputDate)), " +
                             "TransactionPlus15 = (select count(*) from [Transaction] where day(TransactionDate) > 15 and StoreCode = tr.StoreCode and Month(TransactionDate) = MONTH(@InputDate) and YEAR(TransactionDate) = YEAR(@InputDate))," +
                             "QtyPlus15 = (select sum(Qty) from [Transaction] where day(TransactionDate) > 15 and StoreCode = tr.StoreCode and Month(TransactionDate) = MONTH(@InputDate) and YEAR(TransactionDate) = YEAR(@InputDate)) " +
                             "from [Transaction] tr inner join Store s on s.Code = tr.StoreCode group by tr.StoreCode, s.Name";

                var inputDate = new ObjectAPIModel();
                inputDate.param = "@InputDate";
                inputDate.typeValue = DbType.Date;
                inputDate.value = Convert.ToDateTime(transDate);
                listParam.Add(inputDate);

                connect.sqlDataRd = connect.ExecuteDataReaderWithParams(cmd, connect.sqlCon(), listParam);

                if (connect.sqlDataRd.HasRows)
                {
                    while (connect.sqlDataRd.Read())
                    {
                        model = new BasketSizeAPIModel();
                        model.code = connect.sqlDataRd["Code"].ToString();
                        model.name = connect.sqlDataRd["Name"] == null ? "" : connect.sqlDataRd["Name"].ToString();
                        model.trns1 = connect.sqlDataRd["TransactionMin15"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["TransactionMin15"].ToString());
                        int result = 0;
                        if (int.TryParse(connect.sqlDataRd["QtyMin15"].ToString(), out result))
                        {
                            model.qty1 = result;
                            if (result > 0)
                                model.bs1 = model.qty1 / model.trns1;
                        }

                        model.trns2 = connect.sqlDataRd["TransactionPlus15"] == null ? 0 : Convert.ToInt32(connect.sqlDataRd["TransactionPlus15"].ToString());
                        if (int.TryParse(connect.sqlDataRd["QtyPlus15"].ToString(), out result))
                        {
                            model.qty2 = result;
                            if (result > 0)
                                model.bs2 = model.qty2 / model.trns2;
                        }

                        model.grandTrns = model.trns1 + model.trns2;
                        model.grandQty = model.qty1 + model.qty2;
                        model.grandBS = model.bs1 + model.bs2;

                        listModel.Add(model);
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
