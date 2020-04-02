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
    [Route("api/ClosingStore")]
    [ApiController]
    public class ClosingStoreController : Controller
    {
        private readonly DB_BIENSI_POSContext _context;

        public ClosingStoreController(DB_BIENSI_POSContext context)
        {
            _context = context;
        }

        //POST: api/PostTransaction
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClosingStoreAPI transactionApi)
        {
            APIResponse response = new APIResponse();

            try
            {                
                ClosingStore closeStore = new ClosingStore();
                closeStore.ClosignTranBal = transactionApi.closingDeposit;
                closeStore.ClosingDeposit = transactionApi.closingDeposit;
                closeStore.ClosingPettyCash = transactionApi.closingDeposit;
                closeStore.ClosingStoreId = transactionApi.closingStoreId;
                closeStore.ClosingTimeStamp = Convert.ToDateTime(transactionApi.closingTimestamp);
                closeStore.DeviceName = transactionApi.deviceName;
                closeStore.DisputePettyCash = transactionApi.disputePettyCash;
                closeStore.DisputeTransBal = transactionApi.disputeTransBal;
                closeStore.EmployeeId = transactionApi.employeeId;
                closeStore.EmployeeName = transactionApi.employeeName;
                closeStore.OpeningDeposit = transactionApi.openingDeposit;
                closeStore.OpeningPettyCash = transactionApi.openingPettyCash;
                closeStore.OpeningTimeStamp = Convert.ToDateTime(transactionApi.openingTimestamp);
                closeStore.OpeningTransBal = transactionApi.openingTransBal;
                closeStore.RealDeposit = transactionApi.realDeposit;
                closeStore.RealPettyCash = transactionApi.realPettyCash;
                closeStore.RealTransBal = transactionApi.realTransBal;
                closeStore.StatusClose = transactionApi.statusClose;
                closeStore.StoreCode = transactionApi.storeCode;
                _context.Add(closeStore);
                _context.SaveChanges();
                this.sequenceNumber(transactionApi);
                //log record
                LogRecord log = new LogRecord();
                log.TimeStamp = DateTime.Now;
                log.Tag = "Closing Store";
                log.Message = JsonConvert.SerializeObject(transactionApi);
                _context.LogRecord.Add(log);

                response.code = "1";
                response.message = "Sucess Add Data";

                return Ok(response);
            }
            catch (Exception e)
            {
                response.code = "0";
                response.message = e.ToString();

                return BadRequest(response);
            }            
        }

        private void sequenceNumber(ClosingStoreAPI transactionApi)
        {
            SequenceNumberLog log = new SequenceNumberLog();
            log.StoreCode = transactionApi.storeCode;
            log.LastNumberSequence = transactionApi.sequenceNumber;
            log.LastTransId = transactionApi.closingStoreId;
            log.Date = DateTime.Now;
            log.TransactionType = "Closing Store";
            _context.SequenceNumberLog.Add(log);
            _context.SaveChanges();
        }
    }
}