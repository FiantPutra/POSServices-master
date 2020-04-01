using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using POSServices.Models;
using POSServices.WebAPIModel;

namespace POSServices.Controllers
{
    [Route("api/StockTake")]
    [ApiController]
    public class StockTakeController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public StockTakeController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] StockTakeAPI transactionApi)
        {
            APIResponse response = new APIResponse();
            try
            {
                StockTake stockTake = new StockTake();
                stockTake.EmployeeId = transactionApi.employeeId;
                stockTake.EmployeeName = transactionApi.employeeName;
                stockTake.Time = DateTime.Now;
                stockTake.StoreId = transactionApi.storeCode;
                _context.StockTake.Add(stockTake);
                //log record
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;
                log.Tag = "Stock take";
                log.Message = JsonConvert.SerializeObject(transactionApi);
                _context.LogRecord.Add(log);
                _context.SaveChanges();
                for (int i = 0; i < transactionApi.stockTakeLines.Count; i++)
                {
                    StockTakeLineAPI lineAPI = transactionApi.stockTakeLines[i];
                    StockTakeLine line = new StockTakeLine();
                    line.ArticleId = lineAPI.article.articleId;
                    line.ArticleName = lineAPI.article.articleName;
                    line.GoodQty = lineAPI.goodQty;
                    line.RejectQty = lineAPI.rejectQty;
                    line.WhGoodQty = lineAPI.whGoodQty;
                    line.WhRejectQty = lineAPI.whRejectQty;
                    line.StockTakeId = stockTake.Id;
                    _context.Add(line);
                    _context.SaveChanges();
                }
                response.code = "1";
                response.message = "Sucess Add Data";
            }
            catch (Exception ex)
            {
                response.code = "0";
                response.message = ex.ToString();
            }

            return Ok(response);
        }
    }
}