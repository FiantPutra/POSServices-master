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
    public class ReportMutasiBarangController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;
        public ReportMutasiBarangController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get([FromQuery]string transactionId)
        {
            MutasiAPIModel model = new MutasiAPIModel();
            string noReturn = "";
            List<MutasiAPIModel> listModel = new List<MutasiAPIModel>();
            List<MutasiLineAPIModel> listModelLine = new List<MutasiLineAPIModel>();
            List<ObjectAPIModel> listParam = new List<ObjectAPIModel>();

            DBSQLConnect connect = new DBSQLConnect();

            try
            {
                connect.sqlCon().Open();
                string cmd = "select* from vInventoryTransaction where TransactionId = @TransactionId";

                var inputDate = new ObjectAPIModel();
                inputDate.param = "@TransactionId";
                inputDate.typeValue = DbType.String;
                inputDate.value = transactionId;
                listParam.Add(inputDate);

                connect.sqlDataRd = connect.ExecuteDataReaderWithParams(cmd, connect.sqlCon(), listParam);

                if (connect.sqlDataRd.HasRows)
                {
                    while (connect.sqlDataRd.Read())
                    {
                        if (noReturn == "" || noReturn != connect.sqlDataRd["Id"].ToString())
                        {
                            if (model.lines != null)
                                listModel.Add(model);

                            model = new MutasiAPIModel();
                            noReturn = connect.sqlDataRd["Id"].ToString();

                            model.noReturn = noReturn;

                            DateTime transDate;
                            if (DateTime.TryParse(connect.sqlDataRd["TransactionDate"].ToString(), out transDate))
                            {
                                model.tanggalRetur = transDate;
                            }

                            model.showroom = connect.sqlDataRd["Warehouse Original Name"].ToString();
                            model.showroomTujuan = connect.sqlDataRd["Warehouse Destination Name"].ToString();
                            model.keterangan = connect.sqlDataRd["Remarks"].ToString();
                            model.status = connect.sqlDataRd["Status"].ToString();
                            model.lines = new List<MutasiLineAPIModel>();

                            var line = new MutasiLineAPIModel();
                            line.articleId = connect.sqlDataRd["ArticleId"].ToString();
                            line.articleName = connect.sqlDataRd["ArticleName"].ToString();
                            line.color = connect.sqlDataRd["Color"].ToString();
                            line.size = connect.sqlDataRd["Size"].ToString();

                            Decimal qtyResult = 0;
                            if (Decimal.TryParse(connect.sqlDataRd["Qty"].ToString(), out qtyResult))
                            {
                                line.qty = qtyResult;
                            }

                            Decimal totalResult = 0;
                            if (Decimal.TryParse(connect.sqlDataRd["Total"].ToString(), out totalResult))
                            {
                                line.total = totalResult;
                            }

                            if (line.qty > 0 && line.total > 0)
                                line.price = line.total / line.qty;

                            model.lines.Add(line);
                        }
                        else
                        {
                            var line = new MutasiLineAPIModel();
                            line.articleId = connect.sqlDataRd["ArticleId"].ToString();
                            line.articleName = connect.sqlDataRd["ArticleName"].ToString();
                            line.color = connect.sqlDataRd["Color"].ToString();
                            line.size = connect.sqlDataRd["Size"].ToString();

                            Decimal qtyResult = 0;
                            if (Decimal.TryParse(connect.sqlDataRd["Qty"].ToString(), out qtyResult))
                            {
                                line.qty = qtyResult;
                            }

                            Decimal totalResult = 0;
                            if (Decimal.TryParse(connect.sqlDataRd["Total"].ToString(), out totalResult))
                            {
                                line.total = totalResult;
                            }

                            if (line.qty > 0 && line.total > 0)
                                line.price = line.total / line.qty;

                            model.lines.Add(line);
                        }
                    }

                    if (model.lines != null)
                        listModel.Add(model);
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
